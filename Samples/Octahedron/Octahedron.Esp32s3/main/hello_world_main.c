/*
 * SPDX-FileCopyrightText: 2010-2022 Espressif Systems (Shanghai) CO LTD
 *
 * SPDX-License-Identifier: CC0-1.0
 */

#include <stdio.h>
#include <inttypes.h>
#include <esp_lcd_panel_rgb.h>
#include <esp_log.h>
#include <esp_lcd_panel_ops.h>
#include <driver/gpio.h>
#include <esp_timer.h>
#include "sdkconfig.h"
#include "freertos/FreeRTOS.h"
#include "freertos/task.h"
#include "esp_chip_info.h"
#include "esp_flash.h"
#include "generated/Dntc_Samples_Octahedron_Common.h"

#define WIDTH (800)
#define HEIGHT (480)

uint16_t *frameBuffer;

void init_lcd(esp_lcd_panel_handle_t *handle) {
    esp_lcd_rgb_panel_config_t panel_config = {
            .data_width = 16,
            .psram_trans_align = 64,
            .num_fbs = 2,
            .bounce_buffer_size_px = 8 * WIDTH,
            .clk_src = LCD_CLK_SRC_DEFAULT,
            .disp_gpio_num = -1,
            .pclk_gpio_num = 42,
            .vsync_gpio_num = 40,
            .hsync_gpio_num = 39,
            .de_gpio_num = 41,
            .data_gpio_nums = {
                    15, 7, 6, 5, 4, 9, 46,
                    3, 8, 16, 1, 14, 21,
                    47, 48, 45
            },
            .timings = {
                    .pclk_hz = (18 * 1000 * 1000),
                    .h_res = WIDTH,
                    .v_res = HEIGHT,
                    // The following parameters should refer to LCD spec
                    .hsync_back_porch = 40,
                    .hsync_front_porch = 20,
                    .hsync_pulse_width = 1,
                    .vsync_back_porch = 8,
                    .vsync_front_porch = 4,
                    .vsync_pulse_width = 1,
                    .flags.pclk_active_neg = true,
            },
            .flags.fb_in_psram = true,
    };

    gpio_config_t bk_gpio_config = {
            .mode = GPIO_MODE_OUTPUT,
            .pin_bit_mask = 1ULL << 2,
    };
    ESP_ERROR_CHECK(gpio_config(&bk_gpio_config));

    ESP_ERROR_CHECK(esp_lcd_new_rgb_panel(&panel_config, handle));
    ESP_ERROR_CHECK(esp_lcd_panel_reset(*handle));
    ESP_ERROR_CHECK(esp_lcd_panel_init(*handle));
    ESP_ERROR_CHECK(gpio_set_level(2, 1));
//    esp_lcd_panel_invert_color(*handle, true);
}

void app_main(void)
{

    frameBuffer = malloc(sizeof(uint16_t) * WIDTH * HEIGHT);
    assert(frameBuffer != NULL);

    esp_lcd_panel_handle_t panel = NULL;
    init_lcd(&panel);

    Dntc_Samples_Octahedron_Common_Camera camera = Dntc_Samples_Octahedron_Common_Camera_Default();
    camera.PixelWidth = WIDTH;
    camera.PixelHeight = HEIGHT;

    int64_t startTime = esp_timer_get_time();
    while(true) {
        int64_t timeNow = esp_timer_get_time();
        float secondsSinceStart = (float)(timeNow - startTime) / 1000.0f / 1000.0f;

        // Clear the frame buffer
        for (int x = 0; x < WIDTH * HEIGHT; x++) {
            frameBuffer[x] = 0;
        }

        SystemUInt16Array array = {.length = WIDTH * HEIGHT, .items = frameBuffer};
        Dntc_Samples_Octahedron_Common_Renderer_Render(array, camera, secondsSinceStart);

        ESP_ERROR_CHECK(esp_lcd_panel_draw_bitmap(panel, 0, 0, WIDTH, HEIGHT, frameBuffer));
    }
}
