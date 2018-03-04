// thread_t.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include <iostream>  
#include <boost/thread.hpp>   
#include "create_thread.h"
#include "operate_thread.h"
#include "mutex_t.h"
#include <boost/uuid/uuid.hpp>
#include <boost/uuid/uuid_generators.hpp>
#include <boost/uuid/uuid_io.hpp>
#include <boost/lexical_cast.hpp>
using namespace boost::uuids;


// 互斥锁、条件变量、信号量
//1.互斥量 从本质上说就是一把锁, 提供对共享资源的保护访问。
//
//2.条件变量 是利用线程间共享的全局变量进行同步的一种机制，
//主要包括两个动作：一个线程等待"条件变量的条件成立"而挂起；
//另一个线程使"条件成立"（给出条件成立信号）。为了防止竞争，
//条件变量的使用总是和一个互斥锁结合在一起。
//(thread库提供两种条件变量对象condition_variable和condition_variable_any
//，一般情况下我们应该使用condition_variable_any，它能够适应更广泛的互斥量类型。)

//3.信号量 用于线程（或进程）间的同步，一个线程完成了某一个动作就通过信号量告诉别的线程，
//别的线程再进行某些动作.
using namespace boost;
using namespace boost::uuids::detail;
int main(int argc, char* argv[])  
{      
	//uuid u;
	//std::fill_n(u.data + 10, 6, 8); //标准算法fill_n直接操纵数组
	//cout<<u<<endl;
	int i = lexical_cast<int> ("1234");
	cout<<i<<endl;
    return 0;  
}