using GUI.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GUI.MVVM.ViewModel {
    class MainViewModel: ObservableObject {

        public RelayCommand HomeViewCommand { get; set; }

        public RelayCommand FeatureViewCommand { get; set; }




        public HomeViewModel HomeVm {  get; set; }
        public FeatureViewModel FeatureVM {  get; set; }

        private object _currentView;

        public object CurrentView {
            get { return _currentView; }
            set 
            {   _currentView = value;
                OnPropertChanged();
            }
        }
        public MainViewModel() {
            HomeVm = new HomeViewModel();
            FeatureVM = new FeatureViewModel();

            CurrentView = HomeVm;

            HomeViewCommand = new RelayCommand(o => {
                CurrentView = HomeVm;

            });

            FeatureViewCommand = new RelayCommand(o => {
                CurrentView = FeatureVM;
            });
        }
    }
}
