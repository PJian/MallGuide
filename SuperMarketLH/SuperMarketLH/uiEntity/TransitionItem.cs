using EntityManagementService.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Telerik.Windows.Controls.TransitionControl;

namespace SuperMarketLH.uiEntity
{
    class TransitionItem : INotifyPropertyChanged
    {
        private ImageObject _img;
        private FrameNavigate _frameNavigatePage;

        private Question _currentQuestion;

        public Question CurrentQuestion
        {
            get { return _currentQuestion; }
            set
            {
                _currentQuestion = value;
                if (this.PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CurrentQuestion"));
                }
            }
        }

        public FrameNavigate FrameNavigatePage
        {
            get { return _frameNavigatePage; }
            set
            {
                _frameNavigatePage = value;
                if (this.PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("FrameNavigatePage"));
                }
            }
        }
        private TransitionProvider _itemTransition;
        public ImageObject Img
        {
            get { return this._img; }
            set
            {
                this._img = value;
                if (this.PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Img"));
                }
            }
        }
        public TransitionProvider ItemTransition
        {
            get { return this._itemTransition; }
            set
            {
                this._itemTransition = value;
                if (this.PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ItemTransition"));
                }

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
