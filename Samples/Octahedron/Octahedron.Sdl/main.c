#include <stdio.h>
#include <stdbool.h>
#include <stdint.h>
#include <SDL2/SDL.h>
#include "generated/Dntc_Samples_Octahedron_Common.h"

#define WIDTH (480)
#define HEIGHT (272)
#define FRAME_TARGET_TIME (1000.0/30.0)

SDL_Window *window;
SDL_Renderer *renderer;
SDL_Texture *texture;
uint16_t *pixelBuffer;
bool isRunning = true;

bool setup(void) {
    pixelBuffer = malloc(sizeof(uint16_t) * WIDTH * HEIGHT);
    if (!pixelBuffer) {
        fprintf(stderr, "Could not allocate pixel buffer\n");
        return false;
    }

    if (SDL_Init(SDL_INIT_EVERYTHING) != 0) {
        fprintf(stderr, "Error initializing SDL: %s.\n", SDL_GetError());
        return false;
    }

    window = SDL_CreateWindow(
            "Dntc Octahedron Test",
            SDL_WINDOWPOS_CENTERED,
            SDL_WINDOWPOS_CENTERED,
            WIDTH,
            HEIGHT,
            SDL_WINDOW_SHOWN);

    if (!window) {
        fprintf(stderr, "Error creating window: %s\n", SDL_GetError());
        return false;
    }

    renderer = SDL_CreateRenderer(window, -1, 0);
    if (!renderer) {
        fprintf(stderr, "Error creating renderer: %s\n", SDL_GetError());
        return false;
    }

    texture = SDL_CreateTexture(
            renderer,
            SDL_PIXELFORMAT_RGB565,
            SDL_TEXTUREACCESS_STREAMING,
            WIDTH,
            HEIGHT);
    if (!texture) {
        fprintf(stderr, "Error creating texture: %s\n", SDL_GetError());
        return false;
    }

    return true;
}

void renderFrame(void) {
    SDL_UpdateTexture(texture, NULL, pixelBuffer, WIDTH * sizeof(uint16_t));
    SDL_RenderCopy(renderer, texture, NULL, NULL);
    SDL_RenderPresent(renderer);
}

void processEvents(void) {
    SDL_Event event;
    while (SDL_PollEvent(&event)) {
        switch (event.type) {
            case SDL_QUIT:
                isRunning = false;
                break;
        }
    }
}

//int main(__attribute__((unused)) int argc, __attribute__((unused))char *argv[]) {
int main() {
    if (!setup()) {
        return 1;
    }

    Dntc_Samples_Octahedron_Common_Camera camera = Dntc_Samples_Octahedron_Common_Camera_Default();
    camera.PixelHeight = HEIGHT;
    camera.PixelWidth = WIDTH;

    SystemUInt16Array array = {.length = HEIGHT * WIDTH, .items = pixelBuffer};

    uint32_t previousFrameTime = 0;
    float totalTime = 0;
    while (isRunning) {
        int timeToWait = FRAME_TARGET_TIME - (SDL_GetTicks() - previousFrameTime);
        if (timeToWait > 0 && timeToWait <= FRAME_TARGET_TIME) {
            SDL_Delay(timeToWait);
        }

        float timeDelta = (SDL_GetTicks() - previousFrameTime) / 1000.0f;
        previousFrameTime = SDL_GetTicks();
        totalTime += timeDelta;

        processEvents();

        for (int x = 0; x < WIDTH * HEIGHT; x++) {
            pixelBuffer[x] = 0;
        }

        Dntc_Samples_Octahedron_Common_Renderer_Render(array, camera, totalTime);

        renderFrame();
    }

    printf("Closing\n");

    return 0;
}
