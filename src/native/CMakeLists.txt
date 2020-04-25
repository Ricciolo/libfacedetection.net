cmake_minimum_required (VERSION 3.8)

add_library(libfacedetection SHARED "wrapper.cpp" "wrapper.h" "facedetectcnn.cpp" "facedetectcnn.h" "facedetectcnn-int8data.cpp" "facedetectcnn-model.cpp")
target_link_libraries(libfacedetection ${LIBS})

SET_TARGET_PROPERTIES(libfacedetection PROPERTIES PREFIX "")

if(ENABLE_AVX2)
	add_definitions(-D_ENABLE_AVX2)
	set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -mavx2 -mfma")
endif()

if(ENABLE_NEON)
	message("Using ENON")
	add_definitions(-D_ENABLE_NEON)
endif()