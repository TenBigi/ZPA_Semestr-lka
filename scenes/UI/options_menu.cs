using Godot;
using System;

public partial class options_menu : CanvasLayer
{
	[Signal] public delegate void BackPressedEventHandler();
	private sound_button windowButton;
	private HSlider sfxSlider;
	private HSlider musicSlider;
	private sound_button backButton;
	public override void _Ready()
	{
		windowButton = GetNode<sound_button>("MarginContainer/PanelContainer/MarginContainer/VBoxContainer/VBoxContainer/WindowOptionContainer/WindowButton");
		backButton = GetNode<sound_button>("MarginContainer/PanelContainer/MarginContainer/VBoxContainer/BackButton");
		sfxSlider = GetNode<HSlider>("MarginContainer/PanelContainer/MarginContainer/VBoxContainer/VBoxContainer/SfxOptionContainer/SfxSlider");
		musicSlider = GetNode<HSlider>("MarginContainer/PanelContainer/MarginContainer/VBoxContainer/VBoxContainer/MusicOptionContainer/MusicSlider");
		
		backButton.Pressed += OnBackPressed;
		windowButton.Pressed += OnWindowButtonPressed;
		sfxSlider.ValueChanged += (value) => OnAudioSliderChanged((float)value, "sfx");
		musicSlider.ValueChanged += (value) => OnAudioSliderChanged((float)value, "music");
		UpdateDisplay();
	}

	public void UpdateDisplay()
	{
		windowButton.Text = "Windowed";
		if (DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen)
			windowButton.Text = "Fullscreen";
		sfxSlider.Value = GetBusVolumePercent("sfx");
		musicSlider.Value = GetBusVolumePercent("music");
	}

	public float GetBusVolumePercent(string busName)
	{
		var busIndex = AudioServer.GetBusIndex(busName);
		var volumeDb = AudioServer.GetBusVolumeDb(busIndex);
		return Mathf.DbToLinear(volumeDb);
	}
	
	public void SetBusVolumePercent(string busName, float percent)
	{
		var busIndex = AudioServer.GetBusIndex(busName);
		var volumeDb = Mathf.LinearToDb(percent);
		AudioServer.SetBusVolumeDb(busIndex, volumeDb);
	}

    public void OnWindowButtonPressed()
    {
        var mode = DisplayServer.WindowGetMode();
		if (mode != DisplayServer.WindowMode.Fullscreen)
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
		else
			DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, false);
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
		UpdateDisplay();
    }

	public void OnAudioSliderChanged(float value, string busName)
	{
		SetBusVolumePercent(busName, value);
	}

	public void OnBackPressed()
	{
		EmitSignal(nameof(BackPressed));
	}
}
