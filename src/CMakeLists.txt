# CMakeList.txt : Top-level CMake project file, do global configuration
# and include sub-projects here.
#
cmake_minimum_required (VERSION 3.8)

option(ENABLE_AVX2 "use avx2" ON)
option(ENABLE_NEON "whether use neon, if use arm please set it on" OFF)

project ("libfacedetection")

# Include sub-projects.
add_subdirectory ("native")

message("AVX2 = ${ENABLE_AVX2}")
message("NEON = ${ENABLE_NEON}")