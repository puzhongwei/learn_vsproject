// thread_t.cpp : 定义控制台应用程序的入口点。
//  参考 http://blog.csdn.net/zhh157/article/details/3208750

#include "stdafx.h"
#include <iostream>  
#include <boost/thread.hpp>   // 包含头文件
#include <boost/date_time.hpp>       

using namespace std;
// 1.最简单方式创建线程
//工作函数
void workerFunc()  
{  
    boost::posix_time::seconds workTime(3);          
    std::cout << "Worker: running" << std::endl;    
      
    boost::this_thread::sleep(workTime);          
    std::cout << "Worker: finished" << std::endl;  
}    

// 创建线程
void TestCreateThread()
{	// 参数可以是函数对象或者函数指针。并且这个函数无参数，并返回void类型。
	 boost::thread workerThread(workerFunc); // 创建线程对象
	 workerThread.join(); //主线程会等待，直到workerThread执行完毕
						//timed_join()阻塞等待线程结束，或者阻塞等待一定的时间段，然后不管线程是否结
						//束都返回。注意，它不必阻塞等待指定的时间长度，如果在这段时间里线程运行结束，
						//即使时间未到它也会返回。
	 
	 //workerThread.timed_join(boost::posix_time::seconds(1));  //最多等待1秒然后返回
	 //workerThread .join();
}



// 传递参数
void f(int i,std::string &s)
{
	cout<<"int parm i is: "<<i<<endl;
	s = "hou";
	cout<<"string parm s is: "<<s<<endl;
}
int t = 0;
string s = "nihao";
void TestCreateThread2()
{
	boost::thread t(f, t, ref(s));
	t.join();
	cout<<s<<endl;
}


//2.用类内部函数在类外部创建线程
class HelloWorld
{
public:
 void hello(const std::string& str)
 {
        std::cout<<"str is: "<<str<<endl;
 }
}; 
  
void TestCreateThread3()
{
	HelloWorld obj; //如果线程需要绑定的函数有参数则需要使用boost::bind
	boost::thread thrd( boost::bind(&HelloWorld::hello,&obj,"Hello world, I'm a thread!" ) ) ;
	thrd.join();
}


//3.在类内部创建线程
class Hello
{
public:
//在这里start()和hello()方法都必须是static方法。
 static void hello()
 {
      std::cout <<
      "Hello world, I'm a thread in class Hello!"
      << std::endl;
 }
 static void start()
 {
  boost::thread thrd(&hello);
  thrd.join();
 }
};

 void TestCreateThread4()
 {
	 Hello::start();
 }

 
 //4.复杂类型对象作为参数来创建线程
 boost::mutex io_mutex; 
 struct Count 
 { 
	 Count(int id) : id(id) { } 

	 void operator()() 
	 { 
		 for (int i = 0; i < 10; ++i) 
		 { 
			 //boost::mutex::scoped_lock lock(io_mutex); 
			 std::cout << id << ": " 
				 << i << "hellohello"<<std::endl; 
		 } 
	 } 
	 int id; 
 }; 
 
void TestCreateThread5()
{
	 boost::thread thrd1(Count(1)); 
     boost::thread thrd2(Count(2)); 
     thrd1.join(); 
     thrd2.join();
}
 


 



