using EntityManagementService.entity;
using EntityManagementService.sqlUtil;
using EntityManageService.sqlUtil;
using SuperMarketLHS.comm;
using SuperMarketLHS.uientity;
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

namespace SuperMarketLHS.page.other
{
    /// <summary>
    /// PageQuestionnaire.xaml 的交互逻辑
    /// </summary>
    public partial class PageQuestionnaire : Page
    {
        private MainWindow rootWin;
        private Question currentQuestion;
        private List<Question> allQuestions;
        private Boolean isRemoteDBUsed = false;
        public PageQuestionnaire()
        {
            InitializeComponent();
        }
        public PageQuestionnaire(MainWindow rootWin)
        {
            InitializeComponent();
            this.rootWin = rootWin;
        }
        private void init()
        {
            //显示初始化数据库按钮
            DBServer server = SqlHelper.getDBServer();
            if (server == null || !server.Used)
            {
                this.allQuestions = SqlHelper.getAllQuestion();
                isRemoteDBUsed = false;
            }
            else
            {
                this.allQuestions = SqlHelperDB.getAllQuestions();
                isRemoteDBUsed = true;
            }


            this.listBox_allQuestion.ItemsSource = this.allQuestions;
            this.currentQuestion = new Question();
            this.grid_question.DataContext = this.currentQuestion;
            PerformanceViewModel vm = new PerformanceViewModel();
            vm.Q1 = getPerformanceDataByQuestionCount(this.currentQuestion);
            this.RadCartesianChart.DataContext = vm;
        }



        private void listBox_allQuestion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.listBox_allQuestion.SelectedItem != null)
            {
                this.currentQuestion = this.listBox_allQuestion.SelectedItem as Question;
                grid_question.DataContext = this.currentQuestion;
                this.listBox_allQuestion.SelectedIndex = -1;

                PerformanceViewModel vm = new PerformanceViewModel();
                vm.Q1 = getPerformanceDataByQuestionCount(this.currentQuestion);
                this.RadCartesianChart.DataContext = vm;
            }
        }


        private IEnumerable<PerformanceData> getPerformanceDataByQuestionCount(Question question)
        {
            IEnumerable<PerformanceData> datas = new List<PerformanceData>() {
                        new PerformanceData(question.ChoiceA, question.ChoiceA, question.ChoiceACount),
                        new PerformanceData(question.ChoiceB, question.ChoiceB, question.ChoiceBCount),
                        new PerformanceData(question.ChoiceC, question.ChoiceC, question.ChoiceCCount),
                        new PerformanceData(question.ChoiceD, question.ChoiceD, question.ChoiceDCount),
                        new PerformanceData(question.ChoiceE, question.ChoiceE, question.ChoiceECount)
                    };
            return datas;
        }
        private void btn_new_Click(object sender, RoutedEventArgs e)
        {
            this.currentQuestion = new Question();
            grid_question.DataContext = this.currentQuestion;
        }

        private void saveQuestion()
        {
            if (this.currentQuestion == null)
            {
                return;
            }
            if (this.currentQuestion.Id > 0)
            {
                update();
                return;
            }
            // int id = SqlHelper.saveQuestion(this.currentQuestion);

            //保存问题ID到数据库中
            if (this.isRemoteDBUsed)
            {
                SqlHelperDB.createQuestion(this.currentQuestion);
            }
            else
            {
                SqlHelper.saveQuestion(this.currentQuestion);
            }

            //SqlHelperDB.insertQuestion(id);

            init();
            // this.currentQuestion.Id = id;
            // this.listBox_allQuestion.SelectedItem = this.currentQuestion;
            this.currentQuestion = new Question();
            grid_question.DataContext = this.currentQuestion;
            MessageBox.Show("保存成功！");
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            saveQuestion();
        }

        private void btn_del_Click(object sender, RoutedEventArgs e)
        {
            if (this.currentQuestion != null)
            {
                if (this.isRemoteDBUsed)
                {
                    //删除数据库中问题数据
                    SqlHelperDB.deleteQuestion(this.currentQuestion.Id);
                }
                else
                {
                    SqlHelper.removeQuestion(this.currentQuestion);
                }

            }
            this.currentQuestion = new Question();
            grid_question.DataContext = this.currentQuestion;
            init();
            MessageBox.Show("删除成功");
        }

        private void btn_update_Click(object sender, RoutedEventArgs e)
        {
            update();
        }

        private void update()
        {
            if (this.currentQuestion == null) return;
            if (this.currentQuestion.Id > 0)
            {
                if (this.isRemoteDBUsed)
                {
                    SqlHelperDB.updateQuestionContent(this.currentQuestion);
                }
                else
                {
                    SqlHelper.updateQuestion(this.currentQuestion);
                }
                // 

                MessageBox.Show("更新成功!");
            }
            else
            {
                saveQuestion();
                return;
            }
        }

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            init();
        }



    }
}
