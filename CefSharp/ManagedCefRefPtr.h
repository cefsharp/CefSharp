#pragma once

template <typename T>
ref class MCefRefPtr 
{

public:

  MCefRefPtr() : _ptr(NULL) {}

  MCefRefPtr(T* p) : _ptr(p) 
  {
    if (_ptr)
    {
        _ptr->AddRef();
    }
  }

  MCefRefPtr(const MCefRefPtr<T>% r) : _ptr(r._ptr) 
  {
    if (_ptr)
    {
        _ptr->AddRef();
    }
  }

  MCefRefPtr(const CefRefPtr<T>& r) : _ptr(r.get()) 
  {
    if (_ptr)
    {
        _ptr->AddRef();
    }
  }

  ~MCefRefPtr() 
  {
      if (_ptr)
      {
          _ptr->Release();
      }
  }

  !MCefRefPtr()
  {
      if (_ptr)
      {
          _ptr->Release();
      }
  }

  T* get() 
  { 
      return _ptr; 
  }
  
  /*
  operator T*()  // commented out for now as this operator interferes with the return statement of MCefRefPtr<T>% operator=(T* p)
  { 
      return _ptr; 
  }*/
  
  T* operator->() 
  {
      return _ptr; 
  }

  MCefRefPtr<T>% operator=(T* p) 
  {
      // AddRef first so that self assignment should work
      if (p)
      {
          p->AddRef();
      }
      if (_ptr )
      {
          _ptr->Release();
      }
      _ptr = p;
      return *this;
  }

  MCefRefPtr<T>% operator=(const MCefRefPtr<T>% r) 
  {
      return %this = r._ptr;
  }

  void swap(T** pp) 
  {
      T* p = _ptr;
      _ptr = *pp;
      *pp = p;
  }

  void swap(MCefRefPtr<T>% r) 
  {
      swap(%r._ptr);
  }

private:
    T* _ptr;
};

