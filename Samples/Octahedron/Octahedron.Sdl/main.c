#include <stdio.h>
#include <stdbool.h>
#include <stdint.h>
#include <SDL2/SDL.h>

#define WIDTH (800)
#define HEIGHT (480)
#define FRAME_TARGET_TIME (1000/30)

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

    for (int x = 0; x < WIDTH * HEIGHT; x++) {
        pixelBuffer[x] = 0b11111000000000000;
    }

    uint32_t previousFrameTime = 0;
    while (isRunning) {
        int timeToWait = FRAME_TARGET_TIME - (SDL_GetTicks() - previousFrameTime);
        if (timeToWait > 0 && timeToWait <= FRAME_TARGET_TIME) {
            SDL_Delay(timeToWait);
        }

        float timeDelta = (SDL_GetTicks() - previousFrameTime) / 1000.0f;
        previousFrameTime = SDL_GetTicks();

        processEvents();
        renderFrame();
    }

    printf("Closing\n");

    return 0;
}
