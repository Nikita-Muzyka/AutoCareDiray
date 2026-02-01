namespace AutoCareDiray.View;
using AutoCareDiray.ViewModels.Diagnostics;

public partial class DiagnosticsView : ContentPage
{
	public DiagnosticsView(DiagnosticsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}