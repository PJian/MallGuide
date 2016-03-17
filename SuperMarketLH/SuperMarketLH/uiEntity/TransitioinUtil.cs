using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows.Controls.TransitionControl;
using Telerik.Windows.Controls.TransitionEffects;

namespace SuperMarketLH.uiEntity
{
    class TransitioinUtil
    {
        private static TransitionProvider[] TRANSITION_DATA_BASE = { //预定义各种切换特效
                                                                       new FadeTransition(),
                                                                       new WaveTransition(), 
                                                                       new FlipWarpTransition(), 
                                                                       new LayoutClipTransition(),
                                                                       new LinearFadeTransition(), 
                                                                       new PerspectiveRotationTransition(),
                                                                       new PixelateTransition(),
                                                                       new RollTransition(), 
                                                                       new MotionBlurredZoomTransition() 
                                                                   };
        
        
        
        
        /// <summary>
        /// 从切换动画库中随机放回一个
        /// </summary>
        /// <returns></returns>
        public static TransitionProvider getNewTransition()
        {
            Random ran = new Random();
            int RandKey = ran.Next(0, TRANSITION_DATA_BASE.Length - 1);
            return TRANSITION_DATA_BASE[RandKey];
        }

        /// <summary>
        /// 从切换动画库中取出FlipWarp
        /// </summary>
        /// <returns></returns>
        public static TransitionProvider getFlipTransition()
        {
            return TRANSITION_DATA_BASE[3];
        }

        /// <summary>
        /// 从切换动画库中取出渐隐
        /// </summary>
        /// <returns></returns>
        public static TransitionProvider getFadeTransition()
        {
            return TRANSITION_DATA_BASE[0];
        }

    }
}
