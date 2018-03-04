
//������(Mutexes)
//mutex��ͬһʱ��ֻ������һ���̷߳��ʹ�����Դ����һ���߳���Ҫ���ʹ�����Դʱ��
//�������ȡ���ס��mutex������κ������߳��Ѿ���ס��mutex����ô����������һֱ��������
//ֱ����ס��mutex���߳̽�������ͱ�֤�˹�����Դ����ͬһʱ�䣬ֻ��һ���߳̿��Է��ʡ�

//���һ���߳�����һ��mutex�󣬶�û�н������ͻᷢ������
// Ϊ�ˣ�Boost.Threadsר�Ž�������ƣ��ɲ�ֱ�Ӷ�mutex�������߽�������.

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
			std::cout << id << ": " << i << std::endl; //std::cout������һ��������Դ������ÿ���߳̾�ʹ��ȫ��mutex��
														//��ȷ����ͬһʱ�̣�ֻ��һ���߳����������
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

//��������
//��ʱ���������һ��������Դȥʹ�������ǲ����ġ�������Դ�ڱ�ʹ��֮ǰ��
//��ʱ�������봦��ĳ�������״̬�����磬һ���߳��п�����ͼ��һ��ջ����ȡ���ݣ�
//���ջ��û�����ݵĻ�����Ҫ�ȴ��µ����ݵĵ�����mutex��������ͬ��������Ե����������ˡ�
//����һ��ͬ����ʽ������ν�����������������������������Ρ�

//�����������Ǻ�mutex��������Դ����ʹ�á��߳���������mutex��Ȼ����֤������Դ�Ƿ�
//����һ�ֿ��Ա���ȫʹ�õ�״̬�����û�д�������Ҫ��״̬����ô�߳̽��ȴ�����������
//��������ᵼ���ڵȴ��Ĺ����У�mutex���������Ӷ�������һ���߳̿��Ըı乲����Դ��״̬��
//�̴߳ӵȴ���������ʱ��mutex���϶��Ǳ������ġ��������һ���̸߳ı��˹�����Դ��״̬��
//������֪ͨ�������ڵȴ������������̣߳��Ӷ�����ʹ���Ǵӵȴ������з��ء�

const int BUF_SIZE = 10; // ��������С
const int ITERS = 100;  //��д�Ĵ�С���߳���ֹ������
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
		// ����������Ѿ�����
		while (!bStop && buf.size() == BUF_SIZE)
		{
			// ��ע�������ǿ���䣬���߽и�����䣬�涨��lock�������򣬿��Խ����ǿ�����һ�����
			{
				boost::mutex::scoped_lock lock(IO_mutex);
				std::cout << "Buffer is full. Waiting..." << std::endl;
			}
			// ��ע������scoped_lockʹ����RAII���������е��ˣ��Ѿ�������lock�����������
			//       �����������ᱻ�Զ����ã������������������ֵ�����io_mutex��unlock���ʶ�
			//       �������е��ˣ�io_mutex�Ѿ��������ˡ�

			   // ���buffer����
				cond.wait(lock);        // ����lock��������������ǰ�̣߳�ֱ����notify_one()
		}                               // ��notify_all()���ѣ��Ż�������������������lock

		// ���������û����
		buf.push_back(m);              // �뻺�����������ݣ�pΪ��������λ�ñ���
		//p = (p+1) % BUF_SIZE;    
		//++full;                  // full�ӣ�fullΪ��������Ԫ�ص���Ŀ
		cond.notify_one();       // �������ڵ���wait���������������߳��е�һ��
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
			// ����scoped_lockʹ����RAII���������е��ˣ��Ѿ�������lock�����������
			// �����������ᱻ�Զ����ã������������������ֵ�����IO_mutex��unlock���ʶ�
			// �������е��ˣ�IO_mutex�Ѿ��������ˡ�

			     // ���buffer���Ѿ�û��������
				cond.wait(lk);    // ����lk��������������ǰ�̣߳�ֱ����notify_one()
		}                         // ��notify_all()���ѣ��Ż�������������������lk

		int i = buf.back();
		buf.pop_back();            // ȡ���������е�һ������
		//c = (c+1) % BUF_SIZE;              
		//--full;                    // full����fullΪ��������Ԫ�ص���Ŀ
		cond.notify_one();         // �������ڵ���wait���������������߳��е�һ��

		return i;
	}

	void Stop()
	{
		bStop = true;
		cond.notify_all();
	}

private:
	boost::mutex mutex; // ������
	boost::condition cond; // ��������
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
			boost::mutex::scoped_lock lock(io_mutex);  // IO��Դ
			std::cout << "sending: " << n << std::endl;
		}
		buf.put(n); //���뻺����
	}
	cout << "write end." << std::endl;
}

void reader()
{
	for (int x = 0; x < ITERS; ++x)
	{
		int n = buf.get(); //�ӻ�����ȡ
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

