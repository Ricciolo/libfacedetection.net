# CMakeList.txt : CMake project for libfacedetection.net, include source and define
# project specific logic here.
#
cmake_minimum_required (VERSION 3.8)

# Add source to this project's executable.
#add_executable (libfacedetection.net "wrapper.cpp"" "libfacedetection.net.h")

#find_package(facedetection REQUIRED)
#include_directories(${facedetection_INCLUDE_DIRS})
#set(LIBS ${LIBS} ${facedetection_LIBRARIES})

add_library(libfacedetection.net SHARED "wrapper.cpp" "wrapper.h" "facedetectcnn.cpp" "facedetectcnn.h" "facedetectcnn-int8data.cpp" "facedetectcnn-model.cpp")
target_link_libraries(libfacedetection.net ${LIBS})

if(ENABLE_AVX2)
	add_definitions(-D_ENABLE_AVX2)
	set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -mavx2 -mfma")
endif()

if(ENABLE_NEON)
	message("Using ENON")
	add_definitions(-D_ENABLE_NEON)
endif()