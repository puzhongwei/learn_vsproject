
//互斥体(Mutexes)
//mutex在同一时间只能允许一个线程访问共享资源。当一个线程需要访问共享资源时，
//它必须先“锁住”mutex，如果任何其他线程已经锁住了mutex，那么本操作将会一直被阻塞，
//直到锁住了mutex的线程解锁，这就保证了共享资源，在同一时间，只有一个线程可以访问。

//如果一个线程锁定一个mutex后，而没有解锁，就会发生死锁
// 为此，Boost.Threads专门进行了设计，可不直接对mutex加锁或者解锁操作.

#include <boost/thread/thread.hpp>
#include <boost/thread/mutex.hpp>
#include <boost/thread/condition.hpp>
#include <iostream>

using namespace std;

boost::mutex IO_mutex;
 
struct num
{
	num(int id) : id(id) { }
	void operator()()
	{
		for (int i = 0; i < 100; ++i)
		{
			boost::mutex::scoped_lock lock(IO_mutex);
			std::cout << id << ": " << i << std::endl; //std::cout对象是一个共享资源，所以每个线程均使用全局mutex，
														//以确保在同一时刻，只有一个线程输出到它。
		}
	}
	int id;
};

void TestMurex1()
{
	boost::thread thrd1(num(1));
    boost::thread thrd2(num(2));
	boost::thread thrd3(num(3));
    thrd1.join();
    thrd2.join();
	thrd3.join();
}

//条件变量
//有时候仅仅锁定一个共享资源去使用它还是不够的。共享资源在被使用之前，
//有时候它必须处在某种特殊的状态。例如，一个线程有可能试图从一个栈里面取数据，
//如果栈中没有数据的话，它要等待新的数据的到来。mutex处理这种同步问题就显得力不从心了。
//另外一种同步方式，即所谓的条件变量，正好适用于这种情形。

//条件变量总是和mutex、共享资源联合使用。线程首先锁定mutex，然后验证共享资源是否
//处于一种可以被安全使用的状态，如果没有处在所需要的状态，那么线程将等待条件变量。
//这个操作会导致在等待的过程中，mutex被解锁，从而让另外一个线程可以改变共享资源的状态。
//线程从等待操作返回时，mutex将肯定是被锁定的。如果另外一个线程改变了共享资源的状态，
//它必须通知其他正在等待条件变量的线程，从而可以使他们从等待操作中返回。

const int BUF_SIZE = 10; // 缓冲区大小
const int ITERS = 100;  //读写的大小（线程终止条件）
boost::mutex io_mutex;

class buffer
{
public:
	typedef boost::mutex::scoped_lock scoped_lock;

	buffer() : bStop(false)
	{
	}

	void put(int m)
	{
		scoped_lock lock(mutex);
		// 如果缓冲区已经满了
		while (!bStop && buf.size() == BUF_SIZE)
		{
			// 译注：下面是块语句，或者叫复合语句，规定了lock的作用域，可以将它们看做是一条语句
			{
				boost::mutex::scoped_lock lock(IO_mutex);
				std::cout << "Buffer is full. Waiting..." << std::endl;
			}
			// 译注：由于scoped_lock使用了RAII，程序运行到此，已经超出了lock的作用域，因此
			//       其析构函数会被自动调用，而在其析构函数中又调用了io_mutex的unlock，故而
			//       程序运行到此，io_mutex已经被解锁了。

			   // 如果buffer满了
				cond.wait(lock);        // 导致lock解锁，并阻塞当前线程，直到被notify_one()
		}                               // 或notify_all()唤醒，才会解除阻塞，并重新锁定lock

		// 如果缓冲区没有满
		buf.push_back(m);              // 想缓冲区加入数据，p为缓冲区的位置变量
		//p = (p+1) % BUF_SIZE;    
		//++full;                  // full加，full为缓冲区中元素的数目
		cond.notify_one();       // 唤醒由于调用wait而被阻塞的所有线程中的一个
	}

	int get()
	{
		scoped_lock lk(mutex);
		while (!bStop && buf.empty())
		{
			{
				boost::mutex::scoped_lock lock(IO_mutex);
				std::cout << "Buffer is empty. Waiting..." << std::endl;
			}
			// 由于scoped_lock使用了RAII，程序运行到此，已经超出了lock的作用域，因此
			// 其析构函数会被自动调用，而在其析构函数中又调用了IO_mutex的unlock，故而
			// 程序运行到此，IO_mutex已经被解锁了。

			     // 如果buffer中已经没有数据了
				cond.wait(lk);    // 导致lk解锁，并阻塞当前线程，直到被notify_one()
		}                         // 或notify_all()唤醒，才会解除阻塞，并重新锁定lk

		int i = buf.back();
		buf.pop_back();            // 取出缓冲区中的一个数据
		//c = (c+1) % BUF_SIZE;              
		//--full;                    // full减，full为缓冲区中元素的数目
		cond.notify_one();         // 唤醒由于调用wait而被阻塞的所有线程中的一个

		return i;
	}

	void Stop()
	{
		bStop = true;
		cond.notify_all();
	}

private:
	boost::mutex mutex; // 互斥锁
	boost::condition cond; // 条件变量
	//unsigned int p, c, full;
	bool bStop;
	std::vector<int> buf;
};

buffer buf;
void writer()
{
	for (int n = 0; n < ITERS; ++n)
	{
		{
			boost::mutex::scoped_lock lock(io_mutex);  // IO资源
			std::cout << "sending: " << n << std::endl;
		}
		buf.put(n); //放入缓冲区
	}
	cout << "write end." << std::endl;
}

void reader()
{
	for (int x = 0; x < ITERS; ++x)
	{
		int n = buf.get(); //从缓冲区取
		{
			boost::mutex::scoped_lock lock(io_mutex);
			std::cout << "received: " << n << std::endl;
		}
	}
	cout << "read end." << std::endl;
	buf.Stop();

}

void TestMurex2()
{
	boost::thread thrd1(&reader);
	boost::thread thrd2(&writer);
	boost::thread thrd3(&writer);
	thrd1.join();
	thrd2.join();
	thrd3.join();
}

