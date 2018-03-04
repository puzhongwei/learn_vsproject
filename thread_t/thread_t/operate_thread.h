#include "stdafx.h"
#include <iostream>  
#include <boost/thread.hpp>   // 包含头文件
#include <boost/date_time.hpp> 

//yield()函数指示当前线程放弃时间片，允许其他的线程运行。
//sleep()让线程睡眠等待一小段时间，注意它要求参数是一个system_time UTC时间点而不是时间长度。
void Func()  
{  
    boost::posix_time::seconds workTime(3);               
    boost::this_thread::sleep(workTime);
	boost::this_thread::yield();
} 


  /*
  线程中断
  thread的成员函数interrupt()允许正在执行的线程被中断，
  被中断的线程会抛出一个thread interrupted异常，它是一
  个空类，不是std::exception或者boost::exception的子类
  thread_interrupted异常应该在线程执行函数里捕获并处理，
  如果线程不处理这个异常，那么默认的动作是中止线程。
  */

boost::mutex io_mu;
void to_interrupt(int &x, const std::string &str)
{
    try
    {
        for(int i = 0; i < 5; ++i)
        {
            boost::this_thread::sleep(boost::posix_time::seconds(1));
            boost::mutex::scoped_lock lock(io_mu); // 锁定IO流操作
            std::cout << str << ++x << std::endl;
        }
    }
	catch(boost::thread_interrupted&)
    {
        std::cout << "thread_interrupt" << std::endl;
    }
}

void TestOperatrThread1()
{
	int x =0;
    boost::thread t1(to_interrupt, boost::ref(x), "interrupt");

    boost::this_thread::sleep(boost::posix_time::seconds(2));//睡眠2秒钟
    //t1.interrupt();							//要求线程中断执行
    t1.join();								//因为线程已经中断，所以join()立即返回
}

//thread库预定义了共9个中断点，它们都是函数，如下：
//1. thread::join();
//2. thread::timed_join();
//3. condition_variable::wait();
//4. condition_variable::timed_wait();
//5. condition_variable_any::wait();
//6. condition_variable_any::timed_wait();
//7. thread::sleep();
//8. this_thread::sleep();
//9. this_thread::interruption_point()
//这些中断点中的前8个都是某种形式的等待函数，表明线程在阻塞等待的时候可以被中断。
//而最后一个位于子名字空间this_thread的interruption_point()则是一个特殊的中断点函数，
//它并不等待，只是起到一个标签的作用，表示线程执行到这个函数所在的语句就可以被中断。