//
using System;

namespace Helpers.Pagination{
     public class Pagination{
        public int itemsSize;
        private int countButton;
        private float count;

        public Pagination(){
            itemsSize = 5;
        }
        public Pagination(int size){
            itemsSize = size;
        }
        public int pageCalculation(int postCount){
            
            count = (float)postCount/itemsSize-postCount/itemsSize;
            if(count>0){
                countButton = postCount/itemsSize + 1;
            }else{
                countButton = postCount/itemsSize;
            }
            return countButton;
        } 
     }
}