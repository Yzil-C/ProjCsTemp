   M S B A M L   `   `   `  ����F  BGarticPhone, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:GarticPhoneGarticPhone  5   	   =#clr-namespace:GarticPhone.UtilitiesGarticPhone.Utilities  5   	   E'clr-namespace:GarticPhone.GarticWindowsGarticPhone.GarticWindows  5   	       GarticPhone.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:GarticPhone     5   	   4util#clr-namespace:GarticPhone.Utilities     5   	   ;windows'clr-namespace:GarticPhone.GarticWindows     5   	     � Title$  
MainWindow��5
   	   $	��900��6   $
��1800��6)    � DataContext 5   	   �� *Main+   Locator �� Source# [  .��5      � .+�5   
   +   %GarticPhone.GarticWindows.PaintWindow  +   %GarticPhone.GarticWindows.PaintWindow   IsDrawFinished$ OnDrawFinished��6�   y�6   �� *CurrentWindow+  SetWindowVisibility �� 	Converter# [  �� ConverterParameter$ PaintWindow��6�   /   )GarticPhone.GarticWindows.GuessDrawWindow  5   
   y�6"   �� *CurrentWindow+# [ $ GuessDrawWindow��6�   /   )GarticPhone.GarticWindows.FirstTextWindow  5   
   y�6"   �� *CurrentWindow+# [ $ FirstTextWindow��6�   '   !GarticPhone.GarticWindows.PreRoom  5   
   '   !GarticPhone.GarticWindows.PreRoom   UserChoosen$ OnUserChoosen��6�      IsNextIconSelected$ OnNextIconSelected��6�      IsPreviousIconSelected$ OnPreviousIconSelected��6�   y�6   �� *CurrentWindow+# [ $ PreRoom��6  $   GarticPhone.GarticWindows.Room  5   
   $   GarticPhone.GarticWindows.Room	   IsGameStarted$	 OnGameStarted��6�   y�6   �� *CurrentWindow+# [ $
 Room��6�   (	   "GarticPhone.Utilities.TimerControl	  5   
   $
��Left=�6�   $	��Top=�6�   $��1450,10,0,0q�6�   $	��350��6�   $	��100��6�   y�6   �� *CurrentWindow+  SetGameToolsVisibility# [ $ TimerControl��6�   (
   "GarticPhone.Utilities.RoundControl
  5   
   $
��Left=�6�   $	��Top=�6�   y�6   �� *CurrentWindow+# [ $ RoundControl��6�   5      5      