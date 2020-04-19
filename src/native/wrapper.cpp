#include "wrapper.h"

#ifdef _WIN32
# define EXPORTED __declspec(dllexport)
#else
# define EXPORTED
#endif

extern "C"
{
	EXPORTED int Test(AutoString s, int a, int b)
	{
		return a + b;
	}
}