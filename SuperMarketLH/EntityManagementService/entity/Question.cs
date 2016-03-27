using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace EntityManagementService.entity
{
   public class Question 
    {

        private bool isChecked = false;

        public bool IsChecked
        {
            get { return isChecked; }
           // set { isChecked = value; }
        }
       public int Id { get; set; }
       public String Content { get; set; }//题干
       public String ChoiceA { get; set; }//只有五个选项
       public String ChoiceB { get; set; }
       public String ChoiceC { get; set; }
       public String ChoiceD { get; set; }
       public String ChoiceE { get; set; }

       /// <summary>
       /// 人数统计
       /// </summary>
       public int ChoiceACount { get; set; }
       public int ChoiceBCount { get; set; }
       public int ChoiceCCount { get; set; }
       public int ChoiceDCount { get; set; }
       public int ChoiceECount { get; set; }
       public override bool Equals(object obj)
       {
           if (obj is Question)
           {
               Question question = (Question)obj;
               return question.Id == this.Id;
           }
           else {
               return false;
           }
       }
       public override int GetHashCode()
       {
           return this.Id.GetHashCode();
       }

    }
}
