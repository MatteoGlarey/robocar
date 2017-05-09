using System;

public class Resources
{
    private string status
    {
        get { return status; }
        set { status = value; }
    }
    private string position
    {
        get { return position; }
        private set { }
    }
    private Dictionary map;
    private int DIMENSION = 5;

    public Resources()
	{
        status = null;
        
        position = null;
        map = new Dictionary();

        for(int i = 0; i<DIMENSION; i++)
        {
            char row = "A";
            for(int j=0; j<DIMENSION; j++)
            {
                string index = row;
                index += j.ToString();
                map[index] = index;
            }
            row++;
        }       
	}

    public string getPosition(string index)
    {
        return map[index];
    }

}
