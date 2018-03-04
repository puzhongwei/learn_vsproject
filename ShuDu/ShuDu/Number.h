/**
* Êý×Ö¼°×ø±ê¡£
* @author Mars.CN
*
*/
class Number {
private: int row;
private: int col;
private: int value;

 Number(int r , int c , int v){
			 row=r;
			 col=c;
			 value=v;
		 }

public: int getRow() {
			return row;
		}
public: void setRow(int row) {
			this->row = row;
		}
public: int getCol() {
			return col;
		}
public: void setCol(int col) {
			this->col = col;
		}
public: int getValue() {
			return value;
		}
public: void setValue(int value) {
			this->value = value;
		}
}
