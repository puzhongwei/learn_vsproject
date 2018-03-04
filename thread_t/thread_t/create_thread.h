// thread_t.cpp : �������̨Ӧ�ó������ڵ㡣
//  �ο� http://blog.csdn.net/zhh157/article/details/3208750

#include "stdafx.h"
#include <iostream>  
#include <boost/thread.hpp>   // ����ͷ�ļ�
#include <boost/date_time.hpp>       

using namespace std;
// 1.��򵥷�ʽ�����߳�
//��������
void workerFunc()  
{  
    boost::posix_time::seconds workTime(3);          
    std::cout << "Worker: running" << std::endl;    
      
    boost::this_thread::sleep(workTime);          
    std::cout << "Worker: finished" << std::endl;  
}    

// �����߳�
void TestCreateThread()
{	// ���������Ǻ���������ߺ���ָ�롣������������޲�����������void���͡�
	 boost::thread workerThread(workerFunc); // �����̶߳���
	 workerThread.join(); //���̻߳�ȴ���ֱ��workerThreadִ�����
						//timed_join()�����ȴ��߳̽��������������ȴ�һ����ʱ��Σ�Ȼ�󲻹��߳��Ƿ��
						//�������ء�ע�⣬�����������ȴ�ָ����ʱ�䳤�ȣ���������ʱ�����߳����н�����
						//��ʹʱ��δ����Ҳ�᷵�ء�
	 
	 //workerThread.timed_join(boost::posix_time::seconds(1));  //���ȴ�1��Ȼ�󷵻�
	 //workerThread .join();
}



// ���ݲ���
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


//2.�����ڲ����������ⲿ�����߳�
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
	HelloWorld obj; //����߳���Ҫ�󶨵ĺ����в�������Ҫʹ��boost::bind
	boost::thread thrd( boost::bind(&HelloWorld::hello,&obj,"Hello world, I'm a thread!" ) ) ;
	thrd.join();
}


//3.�����ڲ������߳�
class Hello
{
public:
//������start()��hello()������������static������
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

 
 //4.�������Ͷ�����Ϊ�����������߳�
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
 


 



