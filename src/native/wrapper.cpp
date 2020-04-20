#include "wrapper.h"

#include "facedetectcnn.h"

#ifdef _WIN32
# define EXPORTED __declspec(dllexport)
#else
# define EXPORTED
#endif

#define DETECT_BUFFER_SIZE 0x20000

extern "C"
{
	EXPORTED Wrapper* Ctor()
	{
		return new Wrapper();
	}

	EXPORTED void Dtor(Wrapper* wrapper)
	{
		if (wrapper)
		{
			delete wrapper;
		}
	}

	EXPORTED int Detect(Wrapper* wrapper, unsigned char* data, bool rgb2bgr, int width, int height, int step, DetectCallback callback)
	{
		return wrapper->Detect(data, rgb2bgr, width, height, step, callback);
	}
}

Wrapper::Wrapper()
{
	pBuffer = (unsigned char*)malloc(DETECT_BUFFER_SIZE);
}

Wrapper::~Wrapper()
{
	free(pBuffer);
}

int Wrapper::Detect(unsigned char* data, bool rgb2bgr, int width, int height, int step, DetectCallback callback)
{
	int* pResults = NULL;

	if (rgb2bgr) {
		// Only 24bit is supported
		const int bytesPerPixel = 3;
		for (int y = 0; y < height; y++)
		{
			unsigned char* row = data + (y * step);

			for (int x = 0; x < width; x += bytesPerPixel)
			{
				unsigned char* pixel = row + x;
				unsigned char r = pixel[0];
				pixel[0] = pixel[2];
				pixel[2] = r;
			}
		}
	}

	pResults = facedetect_cnn(pBuffer, data, width, height, step);

	int count = (pResults ? *pResults : 0);
	if (callback)
	{
		for (int i = 0; i < count; i++)
		{
			short* p = ((short*)(pResults + 1)) + 142 * i;
			int confidence = p[0];
			int x = p[1];
			int y = p[2];
			int w = p[3];
			int h = p[4];

			FaceDetected fd = { x, y, w, h, confidence / 100.f };
			callback(&fd);
		}
	}

	return count;
}