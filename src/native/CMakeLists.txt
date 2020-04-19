# CMakeList.txt : CMake project for libfacedetection.net, include source and define
# project specific logic here.
#
cmake_minimum_required (VERSION 3.8)

# Add source to this project's executable.
#add_executable (libfacedetection.net "wrapper.cpp"" "libfacedetection.net.h")

find_package(libfacedetection REQUIRED)
include_directories(${libfacedetection_INCLUDE_DIRS})
set(LIBS ${LIBS} ${libfacedetection_LIBRARIES})

target_link_libraries(libfacedetection.net ${LIBS})
add_library(libfacedetection.net SHARED "wrapper.cpp" "wrapper.h")