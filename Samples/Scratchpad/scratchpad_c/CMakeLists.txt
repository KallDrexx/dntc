cmake_minimum_required(VERSION 3.29)
project(scratchpad_c C)

set(CMAKE_C_STANDARD 11)

AUX_SOURCE_DIRECTORY(generated GeneratedSources)
add_executable(scratchpad_c main.c ${GeneratedSources}
        native_test.c
        native_test.h)

target_link_libraries(scratchpad_c m)
