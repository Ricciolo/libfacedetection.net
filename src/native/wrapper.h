#ifndef WRAPPER_H
#define WRAPPER_H

#ifdef _WIN32

typedef const wchar_t* AutoString;

#else

typedef const char* AutoString;

#endif

#endif // !WRAPPER_H
