using System;
using System.Collections.Generic;
using System.Linq;

namespace TestUserInterface
{
    /// <summary> ******************************************************************************************************************
    ///     
    ///                                             Welcome to Menu_UserInterface.cs
    ///                                                             
    ///                       This File holes majority of classes / methords too do with Menu functions 
    ///                       
    ///                         This is pretty much the orginal classes and methods from blackboard files 
    ///                                                                               
    /// </summary>******************************************************************************************************************

    //the menu class, used too create and display a list of menu options  
    public class Menu_UserInterface
    {
        class MenuItem
        {
            private string item;
            private Action selected;

            public MenuItem(string item, Action eventHandler)
            {
                this.item = item;
                selected = eventHandler;
            }

            public void select()
            {
                selected();
            }

            public override string ToString()
            {
                return item;
            }
        }

        private List<MenuItem> items = new List<MenuItem>();

        public void Add(string menuItem, Action eventHandler)
        {
            items.Add(new MenuItem(menuItem, eventHandler));
        }

        public void Display()
        {
            UserInterface.DisplayList("Please select one of the following:", items);
            var option = UserInterface.GetOption(1, items.Count);
            items[option].select();
        }
    }
}
