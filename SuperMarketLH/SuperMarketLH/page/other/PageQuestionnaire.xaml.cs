using EntityManagementService.entity;
using EntityManagementService.sqlUtil;
using EntityManageService.sqlUtil;
using SuperMarketLH.uiEntity;
using SuperMarketLH.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SuperMarketLH.page.other
{
    /// <summary>
    /// PageQuestionnaire.xaml 的交互逻辑
    /// </summary>
    public partial class PageQuestionnaire : Page
    {
        public PageQuestionnaire()
        {
            InitializeComponent();
        }
        private TransitionItem transitioniItem = null;

        private List<Question> allQuestion;
        private int currentIndex;
        private Question currentQuestion;

        private int answerIndex = 0;

        private void init()
        {
            this.allQuestion = SqlHelper.getAllQuestion();
            if (this.allQuestion.Count > 0)
            {
                questionGrid.Visibility = Visibility.Visible;
                noQuestion.Visibility = Visibility.Collapsed;
                this.currentIndex = 0;
                selectQuestion(0);
                if (this.allQuestion.Count == 1)
                {
                    this.btn_next.Visibility = Visibility.Hidden;
                }
                else
                {
                    this.btn_next.Visibility = Visibility.Visible;
                }

            }
            else
            {
                questionGrid.Visibility = Visibility.Collapsed;
                noQuestion.Visibility = Visibility.Visible;
            }

        }
        private void setCount(Question question)
        {
            switch (this.answerIndex)
            {
                case 1: question.ChoiceACount++; break;
                case 2: question.ChoiceBCount++; break;
                case 3: question.ChoiceCCount++; break;
                case 4: question.ChoiceDCount++; break;
                case 5: question.ChoiceECount++; break;
            }
        }

        /// <summary>
        /// 从题库中选择出题目
        /// </summary>
        /// <param name="index"></param>
        private void selectQuestion(int index)
        {
            if (this.allQuestion.Count > index)
            {
                if (currentQuestion != null)
                {
                    setCount(this.currentQuestion);
                }
                answerIndex = 0;
                transitioniItem = new TransitionItem()
                {
                    CurrentQuestion = this.allQuestion.ElementAt(index),
                    ItemTransition = TransitioinUtil.getFadeTransition()
                };
                transitionQuestion.DataContext = transitioniItem;
                currentQuestion = this.allQuestion.ElementAt(index);

            }
        }

        private void hiddenNullContentButton()
        {
            

        }
        private void nextQuestion()
        {
            if (this.answerIndex <= 0)
            {
                MessageBox.Show("请选择您的答案！");
                return;
            }
            selectQuestion(++this.currentIndex);
            if (this.currentIndex >= this.allQuestion.Count - 1)
            {
                this.btn_next.Visibility = Visibility.Collapsed;
            }
        }

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            nextQuestion();
            ClosedUtil.isAnyBodyTouched = true;
        }

        private void btn_done_Click(object sender, RoutedEventArgs e)
        {
            saveAnswer();
            ClosedUtil.isAnyBodyTouched = true;
        }

<<<<<<< HEAD
        private void saveAnswer() {
=======
        private void saveAnswer()
        {

>>>>>>> 53cef5d99d237524c19718c82674aa1cdcece79b
            if (this.answerIndex <= 0)
            {
                MessageBox.Show("请选择您的答案！");
                return;
            }
            if (this.currentIndex < this.allQuestion.Count - 1)
            {
<<<<<<< HEAD
=======

>>>>>>> 53cef5d99d237524c19718c82674aa1cdcece79b
                //提前交卷的
                MessageBox.Show("您还有题目未完成！");
            }
            if (this.answerIndex != 0)
            {
                setCount(this.allQuestion.ElementAt(this.currentIndex));
            }
            foreach (Question q in this.allQuestion)
            {
                SqlHelper.setQuestionCount(q);
                //将答案提交到数据库
                SqlHelperDB.setQuestionCount(q);
            }
            questionGrid.Visibility = Visibility.Collapsed;
            questionDone.Visibility = Visibility.Visible;
        }
        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            init();
        }

        private void btn_choiceA_Checked(object sender, RoutedEventArgs e)
        {
            answerIndex = 1;
            ClosedUtil.isAnyBodyTouched = true;
        }

        private void btn_choiceB_Checked(object sender, RoutedEventArgs e)
        {
            answerIndex = 2;
            ClosedUtil.isAnyBodyTouched = true;
        }

        private void btn_choiceC_Checked(object sender, RoutedEventArgs e)
        {
            answerIndex = 3;
            ClosedUtil.isAnyBodyTouched = true;
        }

        private void btn_choiceD_Checked(object sender, RoutedEventArgs e)
        {
            answerIndex = 4;
            ClosedUtil.isAnyBodyTouched = true;
        }

        private void btn_choiceE_Checked(object sender, RoutedEventArgs e)
        {
            answerIndex = 5;
            ClosedUtil.isAnyBodyTouched = true;
        }
    }
}
