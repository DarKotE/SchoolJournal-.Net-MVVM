﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using AcademicPerformance.ClassFolder;
using AcademicPerformance.CommandsFolder;

namespace AcademicPerformance.ViewModelsFolder

{
    public class VMJournal : INotifyPropertyChanged
    {
        private string searchText;
        public VMJournal()
        {
            
            switch (App.RoleUser)
            {
                case Const.RoleValue.Teacher:
                    SaveCommand = new RelayCommand(Save);
                    DeleteCommand = new RelayCommand(Delete);
                    JournalController = new JournalController();
                    DisciplineController = new DisciplineController();
                    EvaluationController = new EvaluationController();
                    LoadData();
                    Filter();
                    break;

                case Const.RoleValue.Student:
                    JournalController = new JournalController();
                    LoadData();
                    Filter();
                    break;

                case Const.RoleValue.Manager:
                case Const.RoleValue.Admin:
                case Const.RoleValue.Director:
                    SaveCommand = new RelayCommand(Save);
                    DeleteCommand = new RelayCommand(Delete);
                    JournalController = new JournalController();
                    DisciplineController = new DisciplineController();
                    EvaluationController = new EvaluationController();
                    StudentController= new StudentController();;
                    TeacherController= new TeacherController();
                    LoadData();
                    Filter();
                    break;
            }
        }

        public DisciplineController DisciplineController { get; }
        public EvaluationController EvaluationController { get; }
        public JournalController JournalController { get; }
        public TeacherController TeacherController { get; }
        public StudentController StudentController { get; }
        public string Message { get; set; }


        private ObservableCollection<JournalModel> filteredJournalList;
        public ObservableCollection<JournalModel> FilteredJournalList
        {
            get => filteredJournalList;
            set
            {
                filteredJournalList = value;
                OnPropertyChanged(nameof(FilteredJournalList));
            }
        }


        private ObservableCollection<JournalModel> journalList;
        public ObservableCollection<JournalModel> JournalList
        {
            get => journalList;
            set
            {
                journalList = value;
                OnPropertyChanged(nameof(JournalList));
            }
        }

        public ObservableCollection<EvaluationModel> EvaluationList { get; set; }

        public ObservableCollection<StudentModel> StudentList { get; set; }

        public ObservableCollection<TeacherModel> TeacherList { get; set; }


        public ObservableCollection<DisciplineModel> DisciplineList { get; set; }


        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                Filter();
                OnPropertyChanged(nameof(SearchText));
            }
        }

        private JournalModel selectedRow;
        public JournalModel SelectedRow
        {
            get => selectedRow;
            set
            {
                selectedRow = value;
                OnPropertyChanged(nameof(SelectedRow));
            }
        }


        public RelayCommand SaveCommand { get; }

        private EvaluationModel selectedNumber;
        public EvaluationModel SelectedNumber
        {
            get => selectedNumber;
            set
            {
                selectedNumber = value;
                if (EvaluationList == null || SelectedNumber == null || SelectedRow == null ||
                    SelectedRow.NumberEvaluation == selectedNumber.NumberEvaluation) return;
                SelectedRow.NameEvaluation = selectedNumber.NameEvaluation;
                SelectedRow.IdEvaluation = selectedNumber.IdEvaluation;
            }
        }

        private DisciplineModel selectedDiscipline;
        public DisciplineModel SelectedDiscipline
        {
            get => selectedDiscipline;
            set
            {
                selectedDiscipline = value;
                if (DisciplineList == null || selectedDiscipline == null || SelectedRow == null ||
                    SelectedRow.NameDiscipline == selectedDiscipline.NameDiscipline) return;
                SelectedRow.NameDiscipline = selectedDiscipline.NameDiscipline;
                SelectedRow.IdDiscipline = selectedDiscipline.IdDiscipline;
            }
        }

        private TeacherModel selectedTeacher;
        public TeacherModel SelectedTeacher
        {
            set
            {
                selectedTeacher = value;
                if (TeacherList == null || selectedTeacher == null || SelectedRow == null ||
                    SelectedRow.FIOTeacher == selectedTeacher.FullName) return;
                SelectedRow.FIOTeacher = selectedTeacher.FullName;
                SelectedRow.IdTeacher = selectedTeacher.IdTeacher;
            }
        }

        private StudentModel selectedStudent;
        public StudentModel SelectedStudent
        {
            set
            {
                selectedStudent = value;
                if (StudentList == null || selectedStudent == null || SelectedRow == null ||
                    SelectedRow.FIOTeacher == selectedStudent.FullName) return;
                SelectedRow.FIOStudent = selectedStudent.FullName;
                SelectedRow.IdStudent = selectedStudent.IdStudent;
            }
        }


        public RelayCommand DeleteCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Filter()
        {
            FilteredJournalList =
                new ObservableCollection<JournalModel>(
                    from item
                        in JournalList
                    where item.NameEvaluation.ToUpper().Contains(SearchText.ToUpper())
                          || item.FIOTeacher.ToUpper().Contains(SearchText.ToUpper())
                          || item.FIOStudent.ToUpper().Contains(SearchText.ToUpper())
                          || item.NameDiscipline.ToUpper().Contains(SearchText.ToUpper())
                          || item.NumberEvaluation.ToString().ToUpper().Contains(SearchText.ToUpper())
                          || item.IdJournal.ToString().ToUpper().Contains(SearchText.ToUpper())
                    select item);
            if (FilteredJournalList.Any()) SelectedRow = FilteredJournalList[0];
        }


        private void LoadData()
        {
            switch (App.RoleUser)
            {
                case Const.RoleValue.Teacher:
                    DisciplineList = new ObservableCollection<DisciplineModel>(DisciplineController.GetAll());
                    EvaluationList = new ObservableCollection<EvaluationModel>(EvaluationController.GetAll());
                    JournalList = new ObservableCollection<JournalModel>(JournalController.GetAll());
                    SelectedRow = new JournalModel();
                    SearchText = "";
                    break;

                case Const.RoleValue.Student:
                    JournalList = new ObservableCollection<JournalModel>(JournalController.GetAll());
                    SelectedRow = new JournalModel();
                    SearchText = "";
                    break;

                case Const.RoleValue.Admin:
                case Const.RoleValue.Director:
                case Const.RoleValue.Manager:

                    DisciplineList = new ObservableCollection<DisciplineModel>(DisciplineController.GetAll());
                    EvaluationList = new ObservableCollection<EvaluationModel>(EvaluationController.GetAll());
                    JournalList = new ObservableCollection<JournalModel>(JournalController.GetAllFull());
                    StudentList= new ObservableCollection<StudentModel>(StudentController.GetAll());
                    TeacherList = new ObservableCollection<TeacherModel>(TeacherController.GetAll());
                    SelectedRow = new JournalModel();
                    SearchText = "";
                    break;

            }
        }


        public void Save(object param)
        {
            var isAllSaved = true;
            foreach (var item in filteredJournalList)
                if (!JournalController.Update(item))
                    isAllSaved = false;

            Message = isAllSaved ? "Изменения сохранены" : "При сохранении произошла ошибка";
            MessageBox.Show(Message);
            LoadData();
        }

        public void Delete(object param)
        {
            var isDeleted = JournalController.Delete(SelectedRow.IdJournal);
            Message = isDeleted ? "Удалено" : "При удалении произошла ошибка";
            MessageBox.Show(Message);
            LoadData();
        }
    }
}