#include "stdafx.h"
#include <iostream>  
#include <boost/thread.hpp>   // ����ͷ�ļ�
#include <boost/date_time.hpp> 

//yield()����ָʾ��ǰ�̷߳���ʱ��Ƭ�������������߳����С�
//sleep()���߳�˯�ߵȴ�һС��ʱ�䣬ע����Ҫ�������һ��system_time UTCʱ��������ʱ�䳤�ȡ�
void Func()  
{  
    boost::posix_time::seconds workTime(3);               
    boost::this_thread::sleep(workTime);
	boost::this_thread::yield();
} 


  /*
  �߳��ж�
  thread�ĳ�Ա����interrupt()��������ִ�е��̱߳��жϣ�
  ���жϵ��̻߳��׳�һ��thread interrupted�쳣������һ
  �����࣬����std::exception����boost::exception������
  thread_interrupted�쳣Ӧ�����߳�ִ�к����ﲶ�񲢴���
  ����̲߳���������쳣����ôĬ�ϵĶ�������ֹ�̡߳�
  */

boost::mutex io_mu;
void to_interrupt(int &x, const std::string &str)
{
    try
    {
        for(int i = 0; i < 5; ++i)
        {
            boost::this_thread::sleep(boost::posix_time::seconds(1));
            boost::mutex::scoped_lock lock(io_mu); // ����IO������
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

    boost::this_thread::sleep(boost::posix_time::seconds(2));//˯��2����
    //t1.interrupt();							//Ҫ���߳��ж�ִ��
    t1.join();								//��Ϊ�߳��Ѿ��жϣ�����join()��������
}

//thread��Ԥ�����˹�9���жϵ㣬���Ƕ��Ǻ��������£�
//1. thread::join();
//2. thread::timed_join();
//3. condition_variable::wait();
//4. condition_variable::timed_wait();
//5. condition_variable_any::wait();
//6. condition_variable_any::timed_wait();
//7. thread::sleep();
//8. this_thread::sleep();
//9. this_thread::interruption_point()
//��Щ�жϵ��е�ǰ8������ĳ����ʽ�ĵȴ������������߳��������ȴ���ʱ����Ա��жϡ�
//�����һ��λ�������ֿռ�this_thread��interruption_point()����һ��������жϵ㺯����
//�������ȴ���ֻ����һ����ǩ�����ã���ʾ�߳�ִ�е�����������ڵ����Ϳ��Ա��жϡ�