   M S B A M L   `   `   `  ����I  EUsbDetectorWpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:UsbDetectorWpfUsbDetectorWpf  5   	       UsbDetectorWpf.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:UsbDetectorWpf     5   	   _ [Microsoft.Xaml.Behaviors, Version=1.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3e+http://schemas.microsoft.com/xaml/behaviors  5   	     � Title$  
MainWindow��5
   	   $��454.455��6   $��428.713��6-    � 	Resources 5         UsbDetectorWpf.Command   Command��%   &	          5   
   6*   5       � DataContext 5      "   UsbDetectorWpf.MainViewModel  5   
   6    *  $Microsoft.Xaml.Behaviors.Interaction   	Behaviors 5      )   #UsbDetectorWpf.DeviceChangeBehavior  5   
      Command# [  6&   6K   .��5      �  � ColumnDefinitions 5   
   �� 5      $��20 �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $��* �6   �� 5      $��20 �6    � RowDefinitions 5   
   �� 5      $��20 �6   �� 5       $
��auto �6   �� 5!      $
��auto �6   �� 5"      $
��auto �6   �� 5#      $
��auto �6   �� 5$      $
��auto �6   �� 5%      $
��auto �6   �� 5&      $
��auto �6   �� 5'      $
��auto �6   �� 5(      $��* �6   �� 5)      $��20 �6   .+�5-   
   �� $��2��6   $��1��6$   $��Device Info��61   $��24(�5.      $��3��6"   $��0, 0, 0, 20q�5/      �� 51   
   $��1��6   $��2��6$   $��SemiBold"�61   $
��IMEI��52      $��Segoe UI)�6    $��0, 0, 0, 10q�66   �� 53   
   -   5         ��Name$ imeiTextBox��53      $��2��6'   $��2��67   $	��150��54      $��10, 0, 0, 10q�6   �� 55   
   $��1��6   $��3��6$   $��SemiBold"�61   $��SN��56      $��Segoe UI)�6   $��0, 0, 0, 10q�64   �� 57   
   -   5        $ serialNumberTextBox��57      $��2��6/   $��3��6?   $	��150��58      $��10, 0, 0, 10q�6   �� 5:   
   $��1��6   $��4��6$   $��SemiBold"�61   $	��OEM��5;      $��Segoe UI)�6   $��0, 0, 0, 10q�65   �� 5<   
   -   5        $ 
oemTextBox��5<      $��2��6&   $��4��66   $	��150��5=      $��10, 0, 0, 10q�6   �� 5>   
   $��1��6   $��5��6$   $��SemiBold"�61   $��MODEL��5?      $��Segoe UI)�6!   $��0, 0, 0, 10q�67   �� 5A   
   -   5        $ modelTextBox��5A      $��2��6(   $��5��68   $	��150��5B      $��10, 0, 0, 10q�6   �� 5D   
   $��1��6   $��6��6$   $��SemiBold"�61   $��CAPACITY��5E      $��Segoe UI)�6$   $��0, 0, 0, 10q�6:   �� 5G   
   -   5        $ capacityTextBox��5G      $��2��6+   $��6��6;   $	��150��5H      $��10, 0, 0, 10q�6   �� 5I   
   $��1��6   $��7��6$   $��SemiBold"�61   $��COLOR��5J      $��Segoe UI)�6!   $��0, 0, 0, 10q�67   �� 5L   
   -   5        $ colorTextBox��5L      $��2��6(   $��7��68   $	��150��5M      $��10, 0, 0, 10q�6   �� 5O   
   $��1��6   $��8��6$   $��SemiBold"�61   $��SOFTWARE VERSION��5P      $��Segoe UI)�6,   $��0, 0, 0, 10q�6B   �� 5Q   
   -   5        $ softwareVersionTextBox��5Q      $��2��62   $��8��6B   $	��150��5R      $��10, 0, 0, 10q�6   5V      5X      