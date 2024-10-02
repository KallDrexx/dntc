cmake_minimum_required(VERSION 3.29)
project(Octahedron_Sdl C)

set(CMAKE_C_STANDARD 11)
find_package(SDL2 CONFIG REQUIRED)

add_executable(Octahedron_Sdl main.c)

target_link_libraries(Octahedron_Sdl
        PRIVATE
        $<TARGET_NAME_IF_EXISTS:SDL2::SDL2main>
        $<IF:$<TARGET_EXISTS:SDL2::SDL2>,SDL2::SDL2,SDL2::SDL2-static>)
