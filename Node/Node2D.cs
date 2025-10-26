namespace SCENeo.Node;

public class Node2D : Node
{
    private Vec2 _position = Vec2.Zero;

    private Vec2 _globalPosition = Vec2.Zero;

    public Vec2 Position
    {
        get { return _position; }
        set
        {
            if (value == _position)
            {
                return;
            }

            if (Parent == null)
            {
                _position = value;
                return;
            }

            _position = value;

            UpdateChildrenGlobalPosition();
        }
    }

    public Vec2 GlobalPosition
    {
        get { return _globalPosition; }
        set
        {
            if (_globalPosition == value)
            {
                return;
            }

            Position += value - _globalPosition;
        }
    }

    private void UpdateChildrenGlobalPosition()
    {
        _globalPosition = Parent == null || Parent is not Node2D parent2D ? _position 
            : parent2D._globalPosition + _position;

        foreach (Node child in Children)
        {
            if (child is Node2D child2D)
            {
                child2D.UpdateChildrenGlobalPosition();
            }
        }
    }
}