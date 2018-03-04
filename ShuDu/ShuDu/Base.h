#include<iostream>
#include<list>
#include"Number.h"
using namespace std;

/**
 * 基座。<br />
 * 数独的基础数字和相关操作方法。
 * 
 * @author Mars.CN
 * 
 */

class Base {
private: list<Number> baseNumbers;
private: int age=1;		//目前是第几代
private: long id=1;		//顺序ID 
private: int separate[1024];		//隔断索引
private: bool hasSeparate;	//是否分割

	/**
	 * 添加一个基础点。
	 * 
	 * @param num
	 */
public: void addBaseNumber(Number num){
		baseNumbers.add(num);
	}
	
	/**
	 * 获得可填数空白大小。
	 * 
	 * @return
	 */
public: int getSize() {
		return 81 - baseNumbers.size();
	}
	
	/**
	 * 计算得分情况。<br />
	 * 总分为432分。<br />
	 * 在每行、每列、每块中出现重复数字的，重复一次减3分（主要为了拉开差距），每组数字最多减8分，共18组数字。
	 * 
	 * @return
	 */
public: int compute(int dna[]){
		int result=432;
		//int result=1152;
		/**
		 * 1. 添加基础位；
		 * 2. 添加基因位；
		 * 3. 计算重复率。
		 */
		int cube[9][9]={0};
		for(int i =0 ;i<baseNumbers.size();i++){
			cube[baseNumbers[i].getRow()-1][baseNumbers[i].getCol()-1]=baseNumbers[i].getValue();
		}
		for(int r=0,i=0 ; r<9 ; r++){
			for(int c=0 ; c<9 ; c++){
				if(cube[r%3*3+c/3][r/3*3+c%3]!=0){
					continue;
				}
				cube[r%3*3+c/3][r/3*3+c%3]=dna[i++];
			}
		}
		
		//计算行
		
		for(int r=0 ; r<9 ; r++){
			int s[10]={0};
			for(int c=0 ; c<9 ; c++){
				s[cube[r][c]-1]++;
			}
			for(int c=0 ; c<9 ; c++){
				result -= (s[c]==0?0:s[c]-1)*3;	
			}
		}
		
		
		//计算列
		
		for(int r=0 ; r<9 ; r++){
			int s[10]={0};
			for(int c=0 ; c<9 ; c++){
				s[cube[c][r]-1]++;
			}
			for(int c=0 ; c<9 ; c++){
				result -= (s[c]==0?0:s[c]-1)*3;
			}
		}
		return result;
	}
	
	/**
	 * 代。<br />
	 * 用来获取当前带的数字。
	 * @return
	 */
public: int getAge() {
		return age;
	}
	
	/**
	 * 繁殖后代的增加。
	 */
public: void addAge() {
		age++;
	}
	
	/**
	 * 获得单品的唯一ID。
	 * @return
	 */
public: long getId() {
		return id++;
	}
	
	/**
	 * 打印基数。
	 */
public: void print(){
			int  cube[9][9] ={0};
		for(int i =0 ;i<baseNumbers.size();i++){
			cube[baseNumbers[i].getRow()-1][baseNumbers[i].getCol()-1]=baseNumbers[i].getValue();
			
		}
		cout<<"  - - - - - - - - - - - - - - - - - -"<<endl;
		for(int i=0 ; i<9 ; i++){
			cout<<" | ";
			for(int f=0 ; f<9 ; f++){
				if(cube[i][f]==0){
					cout<<" _ ";
				}else{
					cout<<" "<<cube[i][f]<<" ";
				}
				if(f%3==2){
					cout<<" | ";
				}
			}
			cout<<endl;;
			if(i%3==2){
				cout<<"  - - - - - - - - - - - - - - - - - -"<<endl;
			}
		}
	}
	
	/**
	 * 根据基础数字获取一个随机的DNA字串
	 * @return
	 */
	public: int *getRandomDNA(){
		int *result = new int[81-baseNumbers.size()];
		int cube[9][9] ={0};
		for(int i =0 ;i<baseNumbers.size();i++){
			cube[baseNumbers[i].getRow()-1][baseNumbers[i].getCol()-1]=baseNumbers[i].getValue();
			
		}
		/**
		 * 分块添加。
		 */
		int  *nums;
		int at=0;
		int count=0;
		list<int> rns;
		
		for(int d=0 ; d<9 ; d++){
			nums = new int[9];
			at=0;
			for(int p=0 ; p<9 ; p++){
				nums[at++] = (int)cube[d%3*3+p/3][d/3*3+p%3];
			}
			list.sort(nums);
			for(int f=1 ; f<=9 ; f++){
				if(list.find(nums, f)<0){
					rns.add(f);
				}
			}
			//加入随机序列
			while(rns.size()>0){
				int rnd =(int)(Math.random()*rns.size());
				result[count++]=rns.get(rnd);
				rns.remove(rnd);
			}
		}
		return result;
	}
	
	/**
	 * 获取每组九宫的分割索引。
	 * 顺序如下：
	 * 1 4 7
	 * 2 5 8 
	 * 3 6 9
	 * @return
	 */
	public static byte[] getSeparate(){
		if(hasSeparate){
			return separate;
		}
		byte[] result = new byte[10];
		result[0]=0;
		int[][] cube = new int[9][9];
		for(Number n:baseNumbers){
			cube[n.getRow()-1][n.getCol()-1]=n.getValue();
			
		}
		int count=0;
		int ac=0;
		for(int d=0 ; d<9 ; d++){
			ac=0;
			for(int p=0 ; p<9 ; p++){
				if(cube[d%3*3+p/3][d/3*3+p%3]!=0){
					ac++;
				}
			}
			result[d+1] = (byte)(count += 9-ac);
		}
		return result;
	}
	public static ArrayList<Number> getBaseNumbers() {
		return baseNumbers;
	}
	
	public static void print(Member m){
		int[][] cube = new int[9][9];
		for(Number n:baseNumbers){
			cube[n.getRow()-1][n.getCol()-1]=n.getValue();
		}
		for(int r=0,i=0 ; r<9 ; r++){
			for(int c=0 ; c<9 ; c++){
				if(cube[r%3*3+c/3][r/3*3+c%3]!=0){
					continue;
				}
				cube[r%3*3+c/3][r/3*3+c%3]=m.getDNA()[i++];
			}
		}
		System.out.println("  - - - - - - - - - - - - - - - - - -");
		for(int i=0 ; i<9 ; i++){
			System.out.print(" | ");
			for(int f=0 ; f<9 ; f++){
				if(cube[i][f]==0){
					System.out.print(" _ ");
				}else{
					System.out.print(" " + cube[i][f] + " ");
				}
				if(f%3==2){
					System.out.print(" | ");
				}
			}
			System.out.println();
			if(i%3==2){
				System.out.println("  - - - - - - - - - - - - - - - - - -");
			}
		}
	}
}
