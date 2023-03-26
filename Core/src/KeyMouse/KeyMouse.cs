using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wtile.Core.Config;
using Wtile.Core.Keybind;
using Wtile.Core.Utils;
using static Wtile.Core.Config.WtileConfig;

namespace Wtile.Core.KeyMouse;

internal enum Direction { LEFT, RIGHT, UP, DOWN }

internal class KeyMouse
{
    private enum MouseClickState { DOWN, UP }

    private const int MOUSEEVENTF_MOVE = 0x0001;
    private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
    private const int MOUSEEVENTF_LEFTUP = 0x0004;
    private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
    private const int MOUSEEVENTF_RIGHTUP = 0x0010;
    private const int MOUSEEVENTF_WHEEL = 0x0800;
    private const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
    private const int MOUSEEVENTF_MIDDLEUP = 0x0040;

    internal static ConfigKeyMouse Config { get; set; } = new();
    private static bool _leftHeld = false;
    private static bool _rightHeld = false;

    internal static int Speed { get; private set; } = 15;


    internal static void MoveMouse(Direction direction)
    {
        switch (direction)
        {
            case Direction.LEFT: ExternalFunctions.mouse_event(MOUSEEVENTF_MOVE, -Speed, 0, 0, 0); break;
            case Direction.RIGHT: ExternalFunctions.mouse_event(MOUSEEVENTF_MOVE, Speed, 0, 0, 0); break;
            case Direction.UP: ExternalFunctions.mouse_event(MOUSEEVENTF_MOVE, 0, -Speed, 0, 0); break;
            case Direction.DOWN: ExternalFunctions.mouse_event(MOUSEEVENTF_MOVE, 0, Speed, 0, 0); break;
        }
    }

    private static void MiddleClick(MouseClickState state)
    {
        if (state == MouseClickState.DOWN)
            ExternalFunctions.mouse_event(MOUSEEVENTF_MIDDLEDOWN, 0, 0, 0, 0);
        else
            ExternalFunctions.mouse_event(MOUSEEVENTF_MIDDLEUP, 0, 0, 0, 0);
    }

    private static void LeftClick(MouseClickState state)
    {
        if (state == MouseClickState.DOWN)
        {
            if (KeybindManager.IsKeyPressed(WtileModKey.LControlKey))
                ExternalFunctions.mouse_event(MOUSEEVENTF_MIDDLEDOWN, 0, 0, 0, 0);
            else
                ExternalFunctions.mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
        }
        else
        {
            if (KeybindManager.IsKeyPressed(WtileModKey.LControlKey))
                ExternalFunctions.mouse_event(MOUSEEVENTF_MIDDLEUP, 0, 0, 0, 0);
            else
                ExternalFunctions.mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }
    }
    private static void RightClick(MouseClickState state)
    {
        if (state == MouseClickState.DOWN)
            ExternalFunctions.mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
        else
            ExternalFunctions.mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
    }

    private static void Scroll(MouseClickState state, int speed)
    {
        if (state == MouseClickState.UP)
            ExternalFunctions.mouse_event(MOUSEEVENTF_WHEEL, 0, 0, speed, 0);
        else
            ExternalFunctions.mouse_event(MOUSEEVENTF_WHEEL, 0, 0, -speed, 0);
    }

    internal static void Update()
    {
        if (KeybindManager.IsKeyPressed(Config.ModKey))
        {
            if (HandleScroll()) return;
            HandleMouseClick();

            if (KeybindManager.IsKeyPressed(Config.Up)) MoveMouse(Direction.UP);
            if (KeybindManager.IsKeyPressed(Config.Down)) MoveMouse(Direction.DOWN);
            if (KeybindManager.IsKeyPressed(Config.Left)) MoveMouse(Direction.LEFT);
            if (KeybindManager.IsKeyPressed(Config.Right)) MoveMouse(Direction.RIGHT);


            if (KeybindManager.IsKeyPressed(Config.SlowDown)) Speed = Config.SlowSpeed;
            else if (KeybindManager.IsKeyPressed(Config.SpeedUp)) Speed = Config.FastSpeed;
            else Speed = Config.NormalSpeed;
        }
    }
    private static bool HandleScroll()
    {
        var speed = Config.ScrollSpeed;
        if (KeybindManager.IsKeyPressed(Config.SlowDown)) speed = Config.ScrollSpeed / 2;
        else if (KeybindManager.IsKeyPressed(Config.SpeedUp)) speed = Config.ScrollSpeed * 2;


        if (KeybindManager.IsKeyPressed(Config.ScrollMode)
            && KeybindManager.IsKeyPressed(Config.Down))
        {
            Scroll(MouseClickState.DOWN, speed);
            return true;
        }
        if (KeybindManager.IsKeyPressed(Config.ScrollMode)
            && KeybindManager.IsKeyPressed(Config.Up))
        {
            Scroll(MouseClickState.UP, speed);
            return true;
        }
        return false;
    }

    private static void HandleMouseClick()
    {
        bool leftClicked = KeybindManager.IsKeyPressed(Config.LeftClick);

        if (leftClicked && !_leftHeld)
        {
            _leftHeld = true;
            LeftClick(MouseClickState.DOWN);
        }
        else if (!leftClicked && _leftHeld)
        {
            _leftHeld = false;
            LeftClick(MouseClickState.UP);
        }

        bool rightClicked = KeybindManager.IsKeyPressed(Config.RightClick);
        if (rightClicked && !_rightHeld)
        {
            _rightHeld = true;
            RightClick(MouseClickState.DOWN);
        }
        else if (!rightClicked && _rightHeld)
        {
            _rightHeld = false;
            RightClick(MouseClickState.UP);
        }
    }
}
