from kivy.app import App
from kivy.lang import Builder
from kivy.uix.gridlayout import GridLayout

Builder.load_file('comicwidgets.kv')
Builder.load_file('toolbox.kv')
Builder.load_file('drawingspace.kv')
Builder.load_file('generaloptions.kv')
Builder.load_file('statusbar.kv')

class ToolBox(GridLayout):
    pass

class ComicCreatorApp(App):
    pass

if __name__=='__main__':
    ComicCreatorApp().run()