﻿using Wtile.Core.Entities;
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
    private const int MOUSEEVENTF_HWHEEL = 0x01000;

    internal static ConfigKeyMouse Config { get; set; } = new();
    private static bool _leftHeld = false;
    private static bool _rightHeld = false;

    internal static int Speed { get; private set; } = 15;


    private static void MoveMouse(Direction direction)
    {
        switch (direction)
        {
            case Direction.LEFT: ExternalFunctions.mouse_event(MOUSEEVENTF_MOVE, -Speed, 0, 0, 0); break;
            case Direction.RIGHT: ExternalFunctions.mouse_event(MOUSEEVENTF_MOVE, Speed, 0, 0, 0); break;
            case Direction.UP: ExternalFunctions.mouse_event(MOUSEEVENTF_MOVE, 0, -Speed, 0, 0); break;
            case Direction.DOWN: ExternalFunctions.mouse_event(MOUSEEVENTF_MOVE, 0, Speed, 0, 0); break;
        }
    }

    private static void LeftClick(MouseClickState state)
    {
        if (state == MouseClickState.DOWN)
        {
            if (KeybindManager.IsKeyPressed(WtileModKey.LControlKey))
            {
                KeybindManager.SendKeyPress(WtileModKey.LControlKey);
                ExternalFunctions.mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                ExternalFunctions.mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                KeybindManager.SendKeyRelease(WtileModKey.LControlKey);
            }
            else
                ExternalFunctions.mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
        }
        else
            ExternalFunctions.mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
    }
    private static void RightClick(MouseClickState state)
    {
        if (state == MouseClickState.DOWN)
            ExternalFunctions.mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
        else
            ExternalFunctions.mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
    }

    private static void Scroll(Direction state, int speed)
    {
        if (state == Direction.UP)
            ExternalFunctions.mouse_event(MOUSEEVENTF_WHEEL, 0, 0, speed, 0);
        else if (state == Direction.DOWN)
            ExternalFunctions.mouse_event(MOUSEEVENTF_WHEEL, 0, 0, -speed, 0);
        else if (state == Direction.LEFT)
            ExternalFunctions.mouse_event(MOUSEEVENTF_HWHEEL, 0, 0, -speed, 0);
        else if (state == Direction.RIGHT)
            ExternalFunctions.mouse_event(MOUSEEVENTF_HWHEEL, 0, 0, speed, 0);
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
        if (!KeybindManager.IsKeyPressed(Config.ScrollMode)) return false;

        var speed = Config.ScrollSpeed;
        if (KeybindManager.IsKeyPressed(Config.SlowDown)) speed = Config.ScrollSpeed / 2;
        else if (KeybindManager.IsKeyPressed(Config.SpeedUp)) speed = Config.ScrollSpeed * 2;

        if (KeybindManager.IsKeyPressed(Config.Down))
        {
            Scroll(Direction.DOWN, speed);
            return true;
        }
        if (KeybindManager.IsKeyPressed(Config.Up))
        {
            Scroll(Direction.UP, speed);
            return true;
        }
        if (KeybindManager.IsKeyPressed(Config.Left))
        {
            Scroll(Direction.LEFT, speed * 2);
            return true;
        }
        if (KeybindManager.IsKeyPressed(Config.Right))
        {
            Scroll(Direction.RIGHT, speed * 2);
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

    internal static void CenterMouseInWindow(Window window)
    {
        var rect = window.Location;

        int centerX = (rect.Left + rect.Right) / 2;
        int centerY = (rect.Top + rect.Bottom) / 2;

        Cursor.Position = new Point(centerX, centerY);
    }

}
