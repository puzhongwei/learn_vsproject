// QueueImplementation.cpp : Defines the entry point for the console application.
//

#include <iostream>
#include <queue>
#include <string>
#include <process.h>
using namespace std;

class task_impl_i
{
public:
	virtual ~task_impl_i(){}
	virtual void run() = 0;
	virtual task_impl_i* fork() = 0;
};

class task_impl_t : public task_impl_i
{
public:
	task_impl_t(task_func_t func_, void* arg_) :
		m_func(func_),
		m_arg(arg_)
	{}

	virtual void run()
	{
		m_func(m_arg);
	}

	virtual task_impl_i* fork()
	{
		return new task_impl_t(m_func, m_arg);
	}

protected:
	task_func_t m_func;
	void*       m_arg;
};

struct task_t
{
	static void dumy(void*){}
	task_t(task_func_t f_, void* d_) :
		task_impl(new task_impl_t(f_, d_))
	{
	}
	task_t(task_impl_i* task_imp_) :
		task_impl(task_imp_)
	{
	}
	task_t(const task_t& src_) :
		task_impl(src_.task_impl->fork())
	{
	}
	task_t()
	{
		task_impl = new task_impl_t(&task_t::dumy, NULL);
	}
	~task_t()
	{
		delete task_impl;
	}
	task_t& operator=(const task_t& src_)
	{
		delete task_impl;
		task_impl = src_.task_impl->fork();
		return *this;
	}

	void run()
	{
		task_impl->run();
	}
	task_impl_i*    task_impl;
};