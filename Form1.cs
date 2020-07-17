using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//UpdateTime:2017/11/22_0849:Remaked structures n Speed up ,feelsGoodMan:D 
namespace Chessing_UI
{
    public partial class Form1 : Form
    {
        public class Chess : PictureBox
        {
            public enum Clan { White, Black }
            public static int chess_Size = 64;

            public Color Color_Default = Color.Transparent;
            public static Color Color_Passed = Color.FromArgb(125, 28, 87, 114);    
            //Every chess who passed must change its color,but not every chess will recover it's back color
            //In other words,not all chess's "Default Color" are same as each other.

            protected int _ID, _row;
            protected Clan _clan;
            protected Char _calumn;
            protected int Clan_ID;
            protected int Clan_ID_e;    //ClanID of Enemy,to White is 2( 12~17 /6 = 2 ),to Black is 1 ( 6~11 /6 = 1)
            public bool Moved = false;

            public Chess(Clan _clanarg)
            {
                this.Width = chess_Size;
                this.Height = chess_Size;
                this.BackColor = Color_Default;
                this.SizeMode = PictureBoxSizeMode.Zoom;
                this._clan = _clanarg;
            }
            public Clan clan
            {
                get { return _clan; }
            }
            public int ID
            {
                get { return _ID; }
                set { _ID = value; }
            }
            public Char calumn
            {
                get { return this._calumn; }
            }
            public int row
            {
                get { return this._row; }
            }
            public int ClanID {
                get { return Clan_ID; }
            }
            public int ClanID_E {
                get { return Clan_ID_e; }
            }

            //Move a chess out of Board and Set its ID as Out-Range(60229)
            public void Entomb()
            {
                this._ID = 60229;
                if (_clan == Clan.White) Valhalla_holy.Seal(this);
                else Valhalla_inferno.Seal(this);
            }

            //Move a chess to a location and Set the data of target location to chess's ID number
            public void SetTo(char _calumnarg, int _rowarg)
            {
                this._calumn = _calumnarg;
                this._row = _rowarg;
                this.Location = new Point(((int)_calumn - 97) * chess_Size, (8 - _row) * chess_Size);
                IDMatrix[8 - _row][_calumn - 97] = this._ID;
            }

            public virtual bool CanMoveTo(char _calumnarg, int _rowarg) { return false; }
            public virtual void EVOlution() { ;}
            public virtual void Hint() { ;}
            public virtual bool HasAnyStep() { return false;}
            protected override void OnMouseEnter(EventArgs e)
            {
                this.BackColor = Color_Passed;
            }
            protected override void OnMouseLeave(EventArgs e)
            {
                this.BackColor = Color_Default;
            }
        }
        public class Pawn : Chess
        {
            private static EvoluTionKing evoluTABLE = new EvoluTionKing();

            public Pawn(Clan clanarg)
                : base(clanarg)
            {
                switch (this.clan)
                {
                    case Clan.White:
                        this.Image = Chessing_UI.Properties.Resources.wP;
                        this.ID = 6;
                        this.Clan_ID = 1;
                        this.Clan_ID_e = 2;
                        break;
                    case Clan.Black:
                        this.Image = Chessing_UI.Properties.Resources.bP;
                        this.ID = 12;
                        this.Clan_ID = 2;
                        this.Clan_ID_e = 1;
                        break;
                }
            }
            public override bool CanMoveTo(char _calumnarg, int _rowarg)
            {
                int Target_ID = IDat_CI(_calumnarg, _rowarg);
                if (Target_ID == 60229 || (Target_ID / 6) == Clan_ID) return false;           //Out of range or Target is ally 

                //So now Target must be enemy or non-chess block (Chess or non-Chess)

                switch (ID % 6)
                {
                    case 0:
                        //Pawn Mode
                        switch (Clan_ID)
                        {
                            case 1:
                                //White
                                if ((_calumnarg == _calumn) && (_rowarg == _row + 1))
                                {
                                    //Front
                                    if (Target_ID == 0) return true;
                                }
                                else if ((_calumnarg == _calumn) && (_rowarg == _row + 2))
                                {
                                    //Two-step
                                    if (Moved) return false;
                                    if ((Target_ID == 0) && (IDat_CI(_calumnarg, _rowarg - 1) == 0)) return true;
                                }
                                else if ((Math.Abs(_calumnarg - _calumn) == 1) && (_rowarg == _row + 1))
                                {
                                    //Front-right or Front-left

                                    if (Target_ID != 0)
                                    {
                                        //Normal step
                                        return true;
                                    }
                                    else if (En_passantable)
                                    {
                                        //EnPassant
                                        if ((WhoJustStep2.calumn == _calumnarg) && (WhoJustStep2.row == _row)) return true;

                                    }
                                }
                                return false;
                            case 2:
                                //Black
                                if ((_calumnarg == _calumn) && (_rowarg == _row - 1))
                                {
                                    //Front
                                    if (Target_ID == 0) return true;
                                }
                                else if ((_calumnarg == _calumn) && (_rowarg == _row - 2))
                                {
                                    //Two-step
                                    if (Moved) return false;
                                    if ((Target_ID == 0) && (IDat_CI(_calumnarg, _rowarg + 1) == 0)) return true;
                                }
                                else if ((Math.Abs(_calumnarg - _calumn) == 1) && (_rowarg == _row - 1))
                                {
                                    //Front-right or Front-left
                                    if (Target_ID != 0)
                                    {
                                        //Normal-step
                                        return true;
                                    }
                                    else if (En_passantable)
                                    {
                                        //EnPassant
                                        if ((WhoJustStep2.calumn == _calumnarg) && (WhoJustStep2.row == _row)) return true;
                                    }
                                }
                                return false;
                            default:
                                return false;
                        }
                    case 1:
                        //Knight Mode
                        if ((Math.Abs(_calumnarg - _calumn) == 2) && (Math.Abs(_rowarg - _row) == 1)) return true;
                        else if ((Math.Abs(_calumnarg - _calumn) == 1) && (Math.Abs(_rowarg - _row) == 2)) return true;
                        return false;
                    case 2:
                        //Rock Mode
                        if (_calumnarg == _calumn)
                        {
                            //Same-calumn
                            int delta = (_rowarg - _row > 0 ? 1 : -1);
                            for (int ro = _row + delta;; ro += delta)
                            {
                                if (ro == _rowarg) return true;
                                else if (IDat_CI(_calumn, ro) != 0) return false;
                            }
                        }
                        else if (_rowarg == _row)
                        {
                            //Same-row
                            int delta = (_calumnarg - calumn > 0 ? 1 : -1);
                            for (int cal = _calumn + delta; ; cal += delta)
                            {
                                if (cal == _calumnarg) return true;
                                else if (IDat_II(cal - 97, 8 - _row) != 0) return false;
                            }
                        }
                        return false;
                    case 3:
                        //Bishop Mode
                        if (Math.Abs(_calumnarg - _calumn) == Math.Abs(_rowarg - _row))
                        {
                            int _cal_Differ = (_calumnarg - _calumn) > 0 ? 1 : -1;
                            int _row_Differ = (_rowarg - _row) > 0 ? 1 : -1;

                            for (int cal = _calumn + _cal_Differ, ro = _row + _row_Differ; ; cal += _cal_Differ, ro += _row_Differ)
                            {
                                if ((ro == _rowarg) && (cal == _calumnarg)) return true;
                                else if (IDat_II(cal - 97, 8 - ro) != 0) return false;
                            }
                        }
                        return false;
                    case 4:
                        //Queen Mode
                        if (_calumnarg == _calumn)
                        {
                            //Same-calumn
                            int delta = (_rowarg - _row > 0 ? 1 : -1);
                            for (int ro = _row + delta; ; ro += delta)
                            {
                                if (ro == _rowarg) return true;
                                else if (IDat_CI(_calumn, ro) != 0) return false;
                            }
                        }
                        else if (_rowarg == _row)
                        {
                            //Same-row
                            int delta = (_calumnarg - calumn > 0 ? 1 : -1);
                            for (int cal = _calumn + delta; ; cal += delta)
                            {
                                if (cal == _calumnarg) return true;
                                else if (IDat_II(cal - 97, 8 - _row) != 0) return false;
                            }
                        }
                        else if (Math.Abs(_calumnarg - _calumn) == Math.Abs(_rowarg - _row))
                        {
                            int _cal_Differ = (_calumnarg - _calumn) > 0 ? 1 : -1;
                            int _row_Differ = (_rowarg - _row) > 0 ? 1 : -1;

                            for (int cal = _calumn + _cal_Differ, ro = _row + _row_Differ; ; cal += _cal_Differ, ro += _row_Differ)
                            {
                                if ((ro == _rowarg) && (cal == _calumnarg)) return true;
                                else if (IDat_II(cal - 97, 8 - ro) != 0) return false;
                            }
                        }
                        return false;
                    default: return false;
                }
            }
            public override void EVOlution()
            {
                evoluTABLE.ShowDialog();
                _ID += evoluTABLE.choise;

                switch(_ID){
                    case 7: this.Image = Chessing_UI.Properties.Resources.wKn;break;
                    case 8: this.Image = Chessing_UI.Properties.Resources.wR; break;
                    case 9: this.Image = Chessing_UI.Properties.Resources.wB; break;
                    case 10: this.Image = Chessing_UI.Properties.Resources.wQ; break;
                    case 13: this.Image = Chessing_UI.Properties.Resources.bKn; break;
                    case 14: this.Image = Chessing_UI.Properties.Resources.bR; break;
                    case 15: this.Image = Chessing_UI.Properties.Resources.bB; break;
                    case 16: this.Image = Chessing_UI.Properties.Resources.bQ; break;
                }
            }
            public override void Hint()
            {
                if (_ID % 6 == 0)
                {
                    //Pawn Mode
                    switch (Clan_ID)
                    {
                        case 1:
                            //White
                            if (IDat_CI(_calumn, _row + 1) == 0){
                                //Front-step n Two-step
                                DrawHint(_calumn, (_row + 1));
                                if ((!this.Moved) && IDat_CI(_calumn, _row + 2) == 0) DrawHint(_calumn, (_row + 2));
                            }

                            if (IDat_II(_calumn - 98, 7 - _row) / 6 == 2) DrawHint(_calumn - 98, 7 - _row);    //Front-Left
                            if (IDat_II(_calumn - 96, 7 - _row) / 6 == 2) DrawHint(_calumn - 96, 7 - _row);    //Front-Right

                            if (En_passantable && WhoJustStep2._row == _row)
                            {
                                if (WhoJustStep2.calumn - _calumn == -1) DrawHint(_calumn - 98, 7 - _row); //Front-Left
                                else if (WhoJustStep2.calumn - _calumn == 1) DrawHint(_calumn - 96, 7 - _row);  //Front-Right
                            }
                            
                            break;
                        case 2:
                            //Black
                            if (IDat_CI(_calumn, _row -1) == 0){
                                //Front-step n Two-step
                                DrawHint(_calumn, (_row - 1));
                                if ((!this.Moved) && IDat_CI(_calumn, _row - 2) == 0) DrawHint(_calumn, (_row - 2));
                            }

                            if (IDat_II(_calumn - 98, 9 - _row) / 6 == 1) DrawHint(_calumn - 98, 9 - _row);    //Front-Left
                            if (IDat_II(_calumn - 96, 9 - _row) / 6 == 1) DrawHint(_calumn - 96, 9 - _row);    //Front-Right

                            if (En_passantable && WhoJustStep2._row == _row)
                            {
                                if (WhoJustStep2.calumn - _calumn == -1) DrawHint(_calumn - 98, 9 - _row); //Front-Left
                                else if (WhoJustStep2.calumn - _calumn == 1) DrawHint(_calumn - 96, 9 - _row);  //Front-Right
                            }
                            break;
                    }
                }
                else if (_ID % 6 == 1)
                {
                   //Knight-Mode
                    if (_calumn > 97) { 
                        //Left
                        if (_row < 7 && (IDat_II(_calumn - 98, 6 - row) / 6 != Clan_ID)) DrawHint(_calumn - 98, 6 - row);    //-1,+2
                        if (_row > 2 && (IDat_II(_calumn - 98, 10 - row) / 6 != Clan_ID)) DrawHint(_calumn - 98, 10 - row);  //-1,-2
                        if (_calumn > 98) {
                            if (_row < 8 && (IDat_II(_calumn - 99, 7 - row) / 6 != Clan_ID)) DrawHint(_calumn - 99, 7 - row);//-2,+1
                            if (_row > 1 && (IDat_II(_calumn - 99, 9 - row) / 6 != Clan_ID)) DrawHint(_calumn - 99, 9 - row);//-2,-1
                        }
                    }
                    if (_calumn < 104) { 
                        //Right
                        if (_row < 7 && (IDat_II(_calumn - 96, 6 - row) / 6 != Clan_ID)) DrawHint(_calumn - 96, 6 - row);    //+1,+2
                        if (_row > 2 && (IDat_II(_calumn - 96, 10 - row) / 6 != Clan_ID)) DrawHint(_calumn - 96, 10 - row);  //+1,-2
                        if (_calumn <103)
                        {
                            if (_row < 8 && (IDat_II(_calumn - 95, 7 - row) / 6 != Clan_ID)) DrawHint(_calumn - 95, 7 - row);//+2,+1
                            if (_row > 1 && (IDat_II(_calumn - 95, 9 - row) / 6 != Clan_ID)) DrawHint(_calumn - 95, 9 - row);//+2,-1
                        }
                    }
                }
                else if (_ID % 6 == 2)
                {
                    //Rock Mode
                    for (int cal_temp = _calumn + 1; ; cal_temp++){
                        //Right
                        int ID_temp = IDat_II(cal_temp - 97, 8 - _row);
                        if (ID_temp == 0) DrawHint(cal_temp - 97, 8 - _row);
                        else
                        {
                            if (ID_temp / 6 == Clan_ID_e) DrawHint(cal_temp - 97, 8 - _row);
                            break;
                        }
                    }
                    for (int cal_temp = _calumn - 1; ; cal_temp--){
                        //Left
                        int ID_temp = IDat_II(cal_temp - 97, 8 - _row);
                        if (ID_temp == 0) DrawHint(cal_temp - 97, 8 - _row);
                        else
                        {
                            if (ID_temp / 6 == Clan_ID_e) DrawHint(cal_temp - 97, 8 - _row);
                            break;
                        }
                    }
                    for (int row_temp = _row + 1; ; row_temp++){
                        //Up
                        int ID_temp = IDat_CI(_calumn, row_temp);
                        if (ID_temp == 0) DrawHint(_calumn, row_temp);
                        else
                        {
                            if (ID_temp / 6 == Clan_ID_e) DrawHint(_calumn, row_temp);
                            break;
                        }
                    }
                    for (int row_temp = _row - 1; ; row_temp--){
                        //Down
                        int ID_temp = IDat_CI(_calumn, row_temp);
                        if (ID_temp == 0) DrawHint(_calumn, row_temp);
                        else
                        {
                            if (ID_temp / 6 == Clan_ID_e) DrawHint(_calumn, row_temp);
                            break;
                        }
                    }
                }
                else if (_ID % 6 == 3)
                {   
                    //Bishop Mode
                    //Top-Left
                    for (int cal_temp = _calumn - 1, row_temp = _row + 1; ; cal_temp--, row_temp++){
                        int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                        if (ID_temp == 0) DrawHint(cal_temp - 97, 8 - row_temp);
                        else
                        {
                            if (ID_temp / 6 == Clan_ID_e) DrawHint(cal_temp - 97, 8 - row_temp);
                            break;
                        }
                    }
                    //Top-Right
                    for (int cal_temp = _calumn +1, row_temp = _row + 1; ; cal_temp++, row_temp++)
                    {
                        int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                        if (ID_temp == 0) DrawHint(cal_temp - 97, 8 - row_temp);
                        else
                        {
                            if (ID_temp / 6 == Clan_ID_e) DrawHint(cal_temp - 97, 8 - row_temp);
                            break;
                        }

                    }
                    //Bottum-Left
                    for (int cal_temp = _calumn - 1, row_temp = _row - 1; ; cal_temp--, row_temp--)
                    {
                        int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                        if (ID_temp == 0) DrawHint(cal_temp - 97, 8 - row_temp);
                        else
                        {
                            if (ID_temp / 6 == Clan_ID_e) DrawHint(cal_temp - 97, 8 - row_temp);
                            break;
                        }

                    }
                    //Bottom-right
                    for (int cal_temp = _calumn + 1, row_temp = _row - 1; ; cal_temp++, row_temp--)
                    {
                        int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                        if (ID_temp == 0) DrawHint(cal_temp - 97, 8 - row_temp);
                        else
                        {
                            if (ID_temp / 6 == Clan_ID_e) DrawHint(cal_temp - 97, 8 - row_temp);
                            break;
                        }

                    }
                }
                else if (_ID % 6 == 4)
                {
                    //Rock Mode
                    for (int cal_temp = _calumn + 1; ; cal_temp++)
                    {
                        //Right
                        int ID_temp = IDat_II(cal_temp - 97, 8 - _row);
                        if (ID_temp == 0) DrawHint(cal_temp - 97, 8 - _row);
                        else
                        {
                            if (ID_temp / 6 == Clan_ID_e) DrawHint(cal_temp - 97, 8 - _row);
                            break;
                        }
                    }
                    for (int cal_temp = _calumn - 1; ; cal_temp--)
                    {
                        //Left
                        int ID_temp = IDat_II(cal_temp - 97, 8 - _row);
                        if (ID_temp == 0) DrawHint(cal_temp - 97, 8 - _row);
                        else
                        {
                            if (ID_temp / 6 == Clan_ID_e) DrawHint(cal_temp - 97, 8 - _row);
                            break;
                        }
                    }
                    for (int row_temp = _row + 1; ; row_temp++)
                    {
                        //Up
                        int ID_temp = IDat_CI(_calumn, row_temp);
                        if (ID_temp == 0) DrawHint(_calumn, row_temp);
                        else
                        {
                            if (ID_temp / 6 == Clan_ID_e) DrawHint(_calumn, row_temp);
                            break;
                        }
                    }
                    for (int row_temp = _row - 1; ; row_temp--)
                    {
                        //Down
                        int ID_temp = IDat_CI(_calumn, row_temp);
                        if (ID_temp == 0) DrawHint(_calumn, row_temp);
                        else
                        {
                            if (ID_temp / 6 == Clan_ID_e) DrawHint(_calumn, row_temp);
                            break;
                        }
                    }

                    //Bishop Mode
                    //Top-Left
                    for (int cal_temp = _calumn - 1, row_temp = _row + 1; ; cal_temp--, row_temp++)
                    {
                        int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                        if (ID_temp == 0) DrawHint(cal_temp - 97, 8 - row_temp);
                        else
                        {
                            if (ID_temp / 6 == Clan_ID_e) DrawHint(cal_temp - 97, 8 - row_temp);
                            break;
                        }

                    }//Top-Right
                    for (int cal_temp = _calumn + 1, row_temp = _row + 1; ; cal_temp++, row_temp++)
                    {
                        int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                        if (ID_temp == 0) DrawHint(cal_temp - 97, 8 - row_temp);
                        else
                        {
                            if (ID_temp / 6 == Clan_ID_e) DrawHint(cal_temp - 97, 8 - row_temp);
                            break;
                        }

                    }
                    //Bottum-Left
                    for (int cal_temp = _calumn - 1, row_temp = _row - 1; ; cal_temp--, row_temp--)
                    {
                        int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                        if (ID_temp == 0) DrawHint(cal_temp - 97, 8 - row_temp);
                        else
                        {
                            if (ID_temp / 6 == Clan_ID_e) DrawHint(cal_temp - 97, 8 - row_temp);
                            break;
                        }

                    }
                    //Bottom-right
                    for (int cal_temp = _calumn + 1, row_temp = _row - 1; ; cal_temp++, row_temp--)
                    {
                        int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                        if (ID_temp == 0) DrawHint(cal_temp - 97, 8 - row_temp);
                        else
                        {
                            if (ID_temp / 6 == Clan_ID_e) DrawHint(cal_temp - 97, 8 - row_temp);
                            break;
                        }
                    }
                }
            }
            public override bool HasAnyStep()
            {
                if (_ID % 6 == 0)
                {
                    //Pawn Mode
                    switch (Clan_ID)
                    {
                        case 1:
                            //White
                            if (IDat_CI(_calumn, _row + 1) == 0)
                            {
                                //Front-step n Two-step
                                if (Valid_Test(_calumn, _row, _calumn, (_row + 1))) return true;
                                if ((!this.Moved) && IDat_CI(_calumn, _row + 2) == 0) 
                                    if (Valid_Test(_calumn, _row, _calumn, (_row + 2))) return true;
                            }

                            if (IDat_II(_calumn - 98, 7 - _row) / 6 == 2) 
                                if (Valid_Test(_calumn, _row, (char)(_calumn-1), (_row + 1))) return true;    //Front-Left
                            if (IDat_II(_calumn - 96, 7 - _row) / 6 == 2) 
                                if (Valid_Test(_calumn, _row, (char)(_calumn+1), (_row + 1))) return true;    //Front-Right

                            if (En_passantable && WhoJustStep2._row == _row){
                                if (WhoJustStep2.calumn - _calumn == -1)
                                {
                                    if (Valid_Test(_calumn, _row, (char)(_calumn - 1), (_row + 1))) return true;    //Front-Left
                                }
                                else if (WhoJustStep2.calumn - _calumn == 1)
                                {
                                    if (Valid_Test(_calumn, _row, (char)(_calumn + 1), (_row + 1))) return true;    //Front-Right
                                }
                            }
                            break;
                        case 2:
                            //Black
                            if (IDat_CI(_calumn, _row - 1) == 0)
                            {
                                //Front-step n Two-step
                                if (Valid_Test(_calumn, _row, _calumn, (_row - 1))) return true;
                                if ((!this.Moved) && IDat_CI(_calumn, _row - 2) == 0) 
                                    if (Valid_Test(_calumn, _row, _calumn, (_row -2))) return true;
                            }

                            if (IDat_II(_calumn - 98, 9 - _row) / 6 == 1)
                                if (Valid_Test(_calumn, _row, (char)(_calumn - 1), (_row - 1))) return true;    //Front-Left
                            if (IDat_II(_calumn - 96, 9 - _row) / 6 == 1)
                                if (Valid_Test(_calumn, _row, (char)(_calumn + 1), (_row - 1))) return true;    //Front-Right

                            if (En_passantable && WhoJustStep2._row == _row){
                                if (WhoJustStep2.calumn - _calumn == -1)
                                {
                                    if (Valid_Test(_calumn, _row, (char)(_calumn - 1), (_row - 1))) return true;    //Front-Left
                                }
                                else if (WhoJustStep2.calumn - _calumn == 1)
                                {
                                    if (Valid_Test(_calumn, _row, (char)(_calumn + 1), (_row - 1))) return true;    //Front-Right
                                }
                            }
                            break;
                    }
                }
                else if (_ID % 6 == 1)
                {
                    //Knight-Mode
                    if (_calumn > 97)
                    {
                        //Left
                        if (_row < 7 && (IDat_II(_calumn - 98, 6 - row) / 6 != Clan_ID))
                            if (Valid_Test(_calumn, _row, (char)(_calumn - 1), (_row + 2))) return true;    //-1,+2
                        if (_row > 2 && (IDat_II(_calumn - 98, 10 - row) / 6 != Clan_ID))
                            if (Valid_Test(_calumn, _row, (char)(_calumn - 1), (_row - 2))) return true;    //-1,-2

                        if (_calumn > 98)
                        {
                            if (_row < 8 && (IDat_II(_calumn - 99, 7 - row) / 6 != Clan_ID))
                                if (Valid_Test(_calumn, _row, (char)(_calumn - 2), (_row + 1))) return true;   //-2,+1
                            if (_row > 1 && (IDat_II(_calumn - 99, 9 - row) / 6 != Clan_ID))
                                if (Valid_Test(_calumn, _row, (char)(_calumn - 2), (_row - 1))) return true;   //-2,-1
                        }
                    }
                    if (_calumn < 104)
                    {
                        //Right
                        if (_row < 7 && (IDat_II(_calumn - 96, 6 - row) / 6 != Clan_ID))
                            if (Valid_Test(_calumn, _row, (char)(_calumn + 1), (_row + 2))) return true;       //+1,+2
                        if (_row > 2 && (IDat_II(_calumn - 96, 10 - row) / 6 != Clan_ID))
                            if (Valid_Test(_calumn, _row, (char)(_calumn + 1), (_row - 2))) return true;     //+1,-2

                        if (_calumn < 103)
                        {
                            if (_row < 8 && (IDat_II(_calumn - 95, 7 - row) / 6 != Clan_ID))
                                if (Valid_Test(_calumn, _row, (char)(_calumn + 2), (_row + 1))) return true;   //+2,+1
                            if (_row > 1 && (IDat_II(_calumn - 95, 9 - row) / 6 != Clan_ID))
                                if (Valid_Test(_calumn, _row, (char)(_calumn + 2), (_row - 1))) return true;   //+2,-1
                        }
                    }
                }
                else if (_ID % 6 == 2)
                {
                    //Rock Mode
                    for (int cal_temp = _calumn + 1; ; cal_temp++)
                    {
                        //Right
                        int ID_temp = IDat_II(cal_temp - 97, 8 - _row);
                        if (ID_temp == 0)
                        {
                            if (Valid_Test(_calumn, _row, (char)(cal_temp), _row)) return true;
                        }
                        else
                        {
                            if (ID_temp / 6 == Clan_ID_e)
                                if (Valid_Test(_calumn, _row, (char)(cal_temp), _row)) return true;
                            break;
                        }
                    }
                    for (int cal_temp = _calumn - 1; ; cal_temp--)
                    {
                        //Left
                        int ID_temp = IDat_II(cal_temp - 97, 8 - _row);
                        if (ID_temp == 0)
                        {
                            if (Valid_Test(_calumn, _row, (char)(cal_temp), _row)) return true;
                        }
                        else
                        {
                            if (ID_temp / 6 == Clan_ID_e)
                                if (Valid_Test(_calumn, _row, (char)(cal_temp), _row)) return true;
                            break;
                        }
                    }
                    for (int row_temp = _row + 1; ; row_temp++)
                    {
                        //Up
                        int ID_temp = IDat_CI(_calumn, row_temp);
                        if (ID_temp == 0)
                        {
                            if (Valid_Test(_calumn, _row, _calumn, row_temp)) return true;
                        }
                        else
                        {
                            if (ID_temp / 6 == Clan_ID_e)
                                if (Valid_Test(_calumn, _row, _calumn, row_temp)) return true;
                            break;
                        }
                    }
                    for (int row_temp = _row - 1; ; row_temp--)
                    {
                        //Down
                        int ID_temp = IDat_CI(_calumn, row_temp);
                        if (ID_temp == 0)
                        {
                            if (Valid_Test(_calumn, _row, _calumn, row_temp)) return true;
                        }
                        else
                        {
                            if (ID_temp / 6 == Clan_ID_e)
                                if (Valid_Test(_calumn, _row, _calumn, row_temp)) return true;
                            break;
                        }
                    }
                }
                else if (_ID % 6 == 3)
                {
                    //Bishop Mode
                    //Top-Left
                    for (int cal_temp = _calumn - 1, row_temp = _row + 1; ; cal_temp--, row_temp++)
                    {
                        int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                        if (ID_temp == 0)
                        {
                            if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                        }
                        else
                        {
                            if (ID_temp / 6 == Clan_ID_e)
                                if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                            break;
                        }
                    }
                    //Top-Right
                    for (int cal_temp = _calumn + 1, row_temp = _row + 1; ; cal_temp++, row_temp++)
                    {
                        int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                        if (ID_temp == 0)
                        {
                            if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                        }
                        else
                        {
                            if (ID_temp / 6 == Clan_ID_e)
                                if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                            break;
                        }

                    }
                    //Bottum-Left
                    for (int cal_temp = _calumn - 1, row_temp = _row - 1; ; cal_temp--, row_temp--)
                    {
                        int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                        if (ID_temp == 0)
                        {
                            if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                        }
                        else
                        {
                            if (ID_temp / 6 == Clan_ID_e)
                                if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                            break;
                        }

                    }
                    //Bottom-right
                    for (int cal_temp = _calumn + 1, row_temp = _row - 1; ; cal_temp++, row_temp--)
                    {
                        int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                        if (ID_temp == 0)
                        {
                            if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                        }
                        else
                        {
                            if (ID_temp / 6 == Clan_ID_e)
                                if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                            break;
                        }
                    }
                }
                else if (_ID % 6 == 4)
                {
                    //Rock Mode
                    for (int cal_temp = _calumn + 1; ; cal_temp++)
                    {
                        //Right
                        int ID_temp = IDat_II(cal_temp - 97, 8 - _row);
                        if (ID_temp == 0)
                        {
                            if (Valid_Test(_calumn, _row, (char)(cal_temp), _row)) return true;
                        }
                        else
                        {
                            if (ID_temp / 6 == Clan_ID_e)
                                if (Valid_Test(_calumn, _row, (char)(cal_temp), _row)) return true;
                            break;
                        }
                    }
                    for (int cal_temp = _calumn - 1; ; cal_temp--)
                    {
                        //Left
                        int ID_temp = IDat_II(cal_temp - 97, 8 - _row);
                        if (ID_temp == 0)
                        {
                            if (Valid_Test(_calumn, _row, (char)(cal_temp), _row)) return true;
                        }
                        else
                        {
                            if (ID_temp / 6 == Clan_ID_e)
                                if (Valid_Test(_calumn, _row, (char)(cal_temp), _row)) return true;
                            break;
                        }
                    }
                    for (int row_temp = _row + 1; ; row_temp++)
                    {
                        //Up
                        int ID_temp = IDat_CI(_calumn, row_temp);
                        if (ID_temp == 0)
                        {
                            if (Valid_Test(_calumn, _row, _calumn, row_temp)) return true;
                        }
                        else
                        {
                            if (ID_temp / 6 == Clan_ID_e)
                                if (Valid_Test(_calumn, _row, _calumn, row_temp)) return true;
                            break;
                        }
                    }
                    for (int row_temp = _row - 1; ; row_temp--)
                    {
                        //Down
                        int ID_temp = IDat_CI(_calumn, row_temp);
                        if (ID_temp == 0)
                        {
                            if (Valid_Test(_calumn, _row, _calumn, row_temp)) return true;
                        }
                        else
                        {
                            if (ID_temp / 6 == Clan_ID_e)
                                if (Valid_Test(_calumn, _row, _calumn, row_temp)) return true;
                            break;
                        }
                    }

                    //Bishop Mode
                    //Top-Left
                    for (int cal_temp = _calumn - 1, row_temp = _row + 1; ; cal_temp--, row_temp++)
                    {
                        int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                        if (ID_temp == 0)
                        {
                            if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                        }
                        else
                        {
                            if (ID_temp / 6 == Clan_ID_e)
                                if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                            break;
                        }
                    }
                    //Top-Right
                    for (int cal_temp = _calumn + 1, row_temp = _row + 1; ; cal_temp++, row_temp++)
                    {
                        int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                        if (ID_temp == 0)
                        {
                            if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                        }
                        else
                        {
                            if (ID_temp / 6 == Clan_ID_e)
                                if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                            break;
                        }

                    }
                    //Bottum-Left
                    for (int cal_temp = _calumn - 1, row_temp = _row - 1; ; cal_temp--, row_temp--)
                    {
                        int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                        if (ID_temp == 0)
                        {
                            if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                        }
                        else
                        {
                            if (ID_temp / 6 == Clan_ID_e)
                                if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                            break;
                        }

                    }
                    //Bottom-right
                    for (int cal_temp = _calumn + 1, row_temp = _row - 1; ; cal_temp++, row_temp--)
                    {
                        int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                        if (ID_temp == 0)
                        {
                            if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                        }
                        else
                        {
                            if (ID_temp / 6 == Clan_ID_e)
                                if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                            break;
                        }
                    }
                }
                //
                return false;
            }
        }
        public class Knight : Chess
        {
            public Knight(Clan clanarg)
                : base(clanarg)
            {
                switch (this.clan)
                {
                    case Clan.White:
                        this.Image = Chessing_UI.Properties.Resources.wKn;
                        this.ID = 7;
                        this.Clan_ID = 1;
                        this.Clan_ID_e = 2;
                        break;
                    case Clan.Black:
                        this.Image = Chessing_UI.Properties.Resources.bKn;
                        this.ID = 13;
                        this.Clan_ID = 2;
                        this.Clan_ID_e = 1;
                        break;
                }
            }
            public override bool CanMoveTo(char _calumnarg, int _rowarg)
            {
                int Target_ID = IDat_CI(_calumnarg, _rowarg);
                if (Target_ID == 60229 || (Target_ID / 6) == Clan_ID) return false;           //Out of range or Target is ally 

                if ((Math.Abs(_calumnarg - _calumn) == 2) && (Math.Abs(_rowarg - _row) == 1)) return true;
                else if ((Math.Abs(_calumnarg - _calumn) == 1) && (Math.Abs(_rowarg - _row) == 2)) return true;
                return false;
            }
            public override void Hint()
            {
                if (_calumn > 97)
                {
                    //Left
                    if (_row < 7 && IDat_II(_calumn - 98, 6 - row) / 6 != Clan_ID) DrawHint(_calumn - 98, 6 - row);    //-1,+2
                    if (_row > 2 && IDat_II(_calumn - 98, 10 - row) / 6 != Clan_ID) DrawHint(_calumn - 98, 10 - row);  //-1,-2
                    if (_calumn > 98)
                    {
                        if (_row < 8 && IDat_II(_calumn - 99, 7 - row) / 6 != Clan_ID) DrawHint(_calumn - 99, 7 - row);//-2,+1
                        if (_row > 1 && IDat_II(_calumn - 99, 9 - row) / 6 != Clan_ID) DrawHint(_calumn - 99, 9 - row);//-2,-1
                    }
                }
                if (_calumn < 104)
                {
                    //Right
                    if (_row < 7 && IDat_II(_calumn - 96, 6 - row) / 6 != Clan_ID) DrawHint(_calumn - 96, 6 - row);    //+1,+2
                    if (_row > 2 && IDat_II(_calumn - 96, 10 - row) / 6 != Clan_ID) DrawHint(_calumn - 96, 10 - row);  //+1,-2
                    if (_calumn < 103)
                    {
                        if (_row < 8 && IDat_II(_calumn - 95, 7 - row) / 6 != Clan_ID) DrawHint(_calumn - 95, 7 - row);//+2,+1
                        if (_row > 1 && IDat_II(_calumn - 95, 9 - row) / 6 != Clan_ID) DrawHint(_calumn - 95, 9 - row);//+2,-1
                    }
                }
            }
            public override bool HasAnyStep()
            {
                //Knight-Mode
                if (_calumn > 97)
                {
                    //Left
                    if (_row < 7 && (IDat_II(_calumn - 98, 6 - row) / 6 != Clan_ID))
                        if (Valid_Test(_calumn, _row, (char)(_calumn - 1), (_row + 2))) return true;    //-1,+2
                    if (_row > 2 && (IDat_II(_calumn - 98, 10 - row) / 6 != Clan_ID))
                        if (Valid_Test(_calumn, _row, (char)(_calumn - 1), (_row - 2))) return true;    //-1,-2

                    if (_calumn > 98)
                    {
                        if (_row < 8 && (IDat_II(_calumn - 99, 7 - row) / 6 != Clan_ID))
                            if (Valid_Test(_calumn, _row, (char)(_calumn - 2), (_row + 1))) return true;   //-2,+1
                        if (_row > 1 && (IDat_II(_calumn - 99, 9 - row) / 6 != Clan_ID))
                            if (Valid_Test(_calumn, _row, (char)(_calumn - 2), (_row - 1))) return true;   //-2,-1
                    }
                }
                if (_calumn < 104)
                {
                    //Right
                    if (_row < 7 && (IDat_II(_calumn - 96, 6 - row) / 6 != Clan_ID))
                        if (Valid_Test(_calumn, _row, (char)(_calumn + 1), (_row + 2))) return true;       //+1,+2
                    if (_row > 2 && (IDat_II(_calumn - 96, 10 - row) / 6 != Clan_ID))
                        if (Valid_Test(_calumn, _row, (char)(_calumn + 1), (_row - 2))) return true;     //+1,-2

                    if (_calumn < 103)
                    {
                        if (_row < 8 && (IDat_II(_calumn - 95, 7 - row) / 6 != Clan_ID))
                            if (Valid_Test(_calumn, _row, (char)(_calumn + 2), (_row + 1))) return true;   //+2,+1
                        if (_row > 1 && (IDat_II(_calumn - 95, 9 - row) / 6 != Clan_ID))
                            if (Valid_Test(_calumn, _row, (char)(_calumn + 2), (_row - 1))) return true;   //+2,-1
                    }
                }
                return false;
            }
        }
        public class Rock : Chess
        {
            public Rock(Clan clanarg)
                : base(clanarg)
            {
                switch (this.clan)
                {
                    case Clan.White:
                        this.Image = Chessing_UI.Properties.Resources.wR;
                        this.ID = 8;
                        this.Clan_ID = 1;
                        this.Clan_ID_e = 2;
                        break;
                    case Clan.Black:
                        this.Image = Chessing_UI.Properties.Resources.bR;
                        this.ID = 14;
                        this.Clan_ID = 2;
                        this.Clan_ID_e = 1;
                        break;
                }
            }
            public override bool CanMoveTo(char _calumnarg, int _rowarg)
            {
                int Target_ID = IDat_CI(_calumnarg, _rowarg);
                if (Target_ID == 60229 || (Target_ID / 6) == Clan_ID) return false;           //Out of range or Target is ally 

                if (_calumnarg == _calumn)
                {
                    //Same-calumn
                    int delta = (_rowarg - _row > 0 ? 1 : -1);
                    for (int ro = _row + delta; ; ro += delta)
                    {
                        if (ro == _rowarg) return true;
                        else if (IDat_CI(_calumn, ro) != 0) return false;
                    }
                }
                else if (_rowarg == _row)
                {
                    //Same-row
                    int delta = (_calumnarg - calumn > 0 ? 1 : -1);
                    for (int cal = _calumn + delta; ; cal += delta)
                    {
                        if (cal == _calumnarg) return true;
                        else if (IDat_II(cal - 97, 8 - _row) != 0) return false;
                    }
                }
                return false;
            }
            public override void Hint()
            {
                for (int cal_temp = _calumn + 1; ; cal_temp++)
                {
                    //Right
                    int ID_temp = IDat_II(cal_temp - 97, 8 - _row);
                    if (ID_temp == 0) DrawHint(cal_temp - 97, 8 - _row);
                    else
                    {
                        if (ID_temp / 6 == Clan_ID_e) DrawHint(cal_temp - 97, 8 - _row);
                        break;
                    }
                }
                for (int cal_temp = _calumn - 1; ; cal_temp--)
                {
                    //Left
                    int ID_temp = IDat_II(cal_temp - 97, 8 - _row);
                    if (ID_temp == 0) DrawHint(cal_temp - 97, 8 - _row);
                    else
                    {
                        if (ID_temp / 6 == Clan_ID_e) DrawHint(cal_temp - 97, 8 - _row);
                        break;
                    }
                }
                for (int row_temp = _row + 1; ; row_temp++)
                {
                    //Up
                    int ID_temp = IDat_CI(_calumn, row_temp);
                    if (ID_temp == 0) DrawHint(_calumn, row_temp);
                    else
                    {
                        if (ID_temp / 6 == Clan_ID_e) DrawHint(_calumn, row_temp);
                        break;
                    }
                }
                for (int row_temp = _row - 1; ; row_temp--)
                {
                    //Down
                    int ID_temp = IDat_CI(_calumn, row_temp);
                    if (ID_temp == 0) DrawHint(_calumn, row_temp);
                    else
                    {
                        if (ID_temp / 6 == Clan_ID_e) DrawHint(_calumn, row_temp);
                        break;
                    }
                }
            }
            public override bool HasAnyStep()
            {
                //Rock Mode
                for (int cal_temp = _calumn + 1; ; cal_temp++)
                {
                    //Right
                    int ID_temp = IDat_II(cal_temp - 97, 8 - _row);
                    if (ID_temp == 0)
                    {
                        if (Valid_Test(_calumn, _row, (char)(cal_temp), _row)) return true;
                    }
                    else
                    {
                        if (ID_temp / 6 == Clan_ID_e)
                            if (Valid_Test(_calumn, _row, (char)(cal_temp), _row)) return true;
                        break;
                    }
                }
                for (int cal_temp = _calumn - 1; ; cal_temp--)
                {
                    //Left
                    int ID_temp = IDat_II(cal_temp - 97, 8 - _row);
                    if (ID_temp == 0)
                    {
                        if (Valid_Test(_calumn, _row, (char)(cal_temp), _row)) return true;
                    }
                    else
                    {
                        if (ID_temp / 6 == Clan_ID_e)
                            if (Valid_Test(_calumn, _row, (char)(cal_temp), _row)) return true;
                        break;
                    }
                }
                for (int row_temp = _row + 1; ; row_temp++)
                {
                    //Up
                    int ID_temp = IDat_CI(_calumn, row_temp);
                    if (ID_temp == 0)
                    {
                        if (Valid_Test(_calumn, _row, _calumn, row_temp)) return true;
                    }
                    else
                    {
                        if (ID_temp / 6 == Clan_ID_e)
                            if (Valid_Test(_calumn, _row, _calumn, row_temp)) return true;
                        break;
                    }
                }
                for (int row_temp = _row - 1; ; row_temp--)
                {
                    //Down
                    int ID_temp = IDat_CI(_calumn, row_temp);
                    if (ID_temp == 0)
                    {
                        if (Valid_Test(_calumn, _row, _calumn, row_temp)) return true;
                    }
                    else
                    {
                        if (ID_temp / 6 == Clan_ID_e)
                            if (Valid_Test(_calumn, _row, _calumn, row_temp)) return true;
                        break;
                    }
                }
                return false;
            }
        }
        public class Bishop : Chess
        {
            public Bishop(Clan clanarg)
                : base(clanarg)
            {
                switch (this.clan)
                {
                    case Clan.White:
                        this.Image = Chessing_UI.Properties.Resources.wB;
                        this.ID = 9;
                        this.Clan_ID = 1;
                        this.Clan_ID_e = 2;
                        break;
                    case Clan.Black:
                        this.Image = Chessing_UI.Properties.Resources.bB;
                        this.ID = 15;
                        this.Clan_ID = 2;
                        this.Clan_ID_e = 1;
                        break;
                }
            }
            public override bool CanMoveTo(char _calumnarg, int _rowarg)
            {
                int Target_ID = IDat_CI(_calumnarg, _rowarg);
                if (Target_ID == 60229 || (Target_ID / 6) == Clan_ID) return false;           //Out of range or Target is ally 

                if (Math.Abs(_calumnarg - _calumn) == Math.Abs(_rowarg - _row))
                {
                    int _cal_Differ = (_calumnarg - _calumn) > 0 ? 1 : -1;
                    int _row_Differ = (_rowarg - _row) > 0 ? 1 : -1;

                    for (int cal = _calumn + _cal_Differ, ro = _row + _row_Differ; ; cal += _cal_Differ, ro += _row_Differ)
                    {
                        if ((ro == _rowarg) && (cal == _calumnarg)) return true;
                        else if (IDat_II(cal - 97, 8 - ro) != 0) return false;
                    }
                }
                return false;
            }
            public override void Hint()
            {
                //Top-Left
                for (int cal_temp = _calumn - 1, row_temp = _row + 1; ; cal_temp--, row_temp++)
                {
                    int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                    if (ID_temp == 0) DrawHint(cal_temp - 97, 8 - row_temp);
                    else
                    {
                        if (ID_temp / 6 == Clan_ID_e) DrawHint(cal_temp - 97, 8 - row_temp);
                        break;
                    }

                }
                //Top-Right
                for (int cal_temp = _calumn + 1, row_temp = _row + 1; ; cal_temp++, row_temp++)
                {
                    int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                    if (ID_temp == 0) DrawHint(cal_temp - 97, 8 - row_temp);
                    else
                    {
                        if (ID_temp / 6 == Clan_ID_e) DrawHint(cal_temp - 97, 8 - row_temp);
                        break;
                    }

                }
                //Bottum-Left
                for (int cal_temp = _calumn - 1, row_temp = _row - 1; ; cal_temp--, row_temp--)
                {
                    int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                    if (ID_temp == 0) DrawHint(cal_temp - 97, 8 - row_temp);
                    else
                    {
                        if (ID_temp / 6 == Clan_ID_e) DrawHint(cal_temp - 97, 8 - row_temp);
                        break;
                    }

                }
                //Bottom-right
                for (int cal_temp = _calumn + 1, row_temp = _row - 1; ; cal_temp++, row_temp--)
                {
                    int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                    if (ID_temp == 0) DrawHint(cal_temp - 97, 8 - row_temp);
                    else
                    {
                        if (ID_temp / 6 == Clan_ID_e) DrawHint(cal_temp - 97, 8 - row_temp);
                        break;
                    }

                }
            }
            public override bool HasAnyStep()
            {
                //Bishop Mode
                //Top-Left
                for (int cal_temp = _calumn - 1, row_temp = _row + 1; ; cal_temp--, row_temp++)
                {
                    int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                    if (ID_temp == 0)
                    {
                        if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                    }
                    else
                    {
                        if (ID_temp / 6 == Clan_ID_e)
                            if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                        break;
                    }
                }
                //Top-Right
                for (int cal_temp = _calumn + 1, row_temp = _row + 1; ; cal_temp++, row_temp++)
                {
                    int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                    if (ID_temp == 0)
                    {
                        if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                    }
                    else
                    {
                        if (ID_temp / 6 == Clan_ID_e)
                            if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                        break;
                    }

                }
                //Bottum-Left
                for (int cal_temp = _calumn - 1, row_temp = _row - 1; ; cal_temp--, row_temp--)
                {
                    int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                    if (ID_temp == 0)
                    {
                        if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                    }
                    else
                    {
                        if (ID_temp / 6 == Clan_ID_e)
                            if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                        break;
                    }

                }
                //Bottom-right
                for (int cal_temp = _calumn + 1, row_temp = _row - 1; ; cal_temp++, row_temp--)
                {
                    int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                    if (ID_temp == 0)
                    {
                        if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                    }
                    else
                    {
                        if (ID_temp / 6 == Clan_ID_e)
                            if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                        break;
                    }
                }
                return false;
            }
        }
        public class Queen : Chess
        {
            public Queen(Clan clanarg)
                : base(clanarg)
            {
                switch (this.clan)
                {
                    case Clan.White:
                        this.Image = Chessing_UI.Properties.Resources.wQ;
                        this.ID = 10;
                        this.Clan_ID = 1;
                        this.Clan_ID_e = 2;
                        break;
                    case Clan.Black:
                        this.Image = Chessing_UI.Properties.Resources.bQ;
                        this.ID = 16;
                        this.Clan_ID = 2;
                        this.Clan_ID_e = 1;
                        break;
                }
            }
            public override bool CanMoveTo(char _calumnarg, int _rowarg)
            {
                int Target_ID = IDat_CI(_calumnarg, _rowarg);
                if (Target_ID == 60229 || (Target_ID / 6) == Clan_ID) return false;           //Out of range or Target is ally 

                if (_calumnarg == _calumn)
                {
                    //Same-calumn
                    int delta = (_rowarg - _row > 0 ? 1 : -1);
                    for (int ro = _row + delta; ; ro += delta)
                    {
                        if (ro == _rowarg) return true;
                        else if (IDat_CI(_calumn, ro) != 0) return false;
                    }
                }
                else if (_rowarg == _row)
                {
                    //Same-row
                    int delta = (_calumnarg - calumn > 0 ? 1 : -1);
                    for (int cal = _calumn + delta; ; cal += delta)
                    {
                        if (cal == _calumnarg) return true;
                        else if (IDat_II(cal - 97, 8 - _row) != 0) return false;
                    }
                }
                else if (Math.Abs(_calumnarg - _calumn) == Math.Abs(_rowarg - _row))
                {
                    int _cal_Differ = (_calumnarg - _calumn) > 0 ? 1 : -1;
                    int _row_Differ = (_rowarg - _row) > 0 ? 1 : -1;

                    for (int cal = _calumn + _cal_Differ, ro = _row + _row_Differ; ; cal += _cal_Differ, ro += _row_Differ)
                    {
                        if ((ro == _rowarg) && (cal == _calumnarg)) return true;
                        else if (IDat_II(cal - 97, 8 - ro) != 0) return false;
                    }
                }
                return false;
            }
            public override void Hint()
            {
                //Rock Mode
                for (int cal_temp = _calumn + 1; ; cal_temp++)
                {
                    //Right
                    int ID_temp = IDat_II(cal_temp - 97, 8 - _row);
                    if (ID_temp == 0) DrawHint(cal_temp - 97, 8 - _row);
                    else
                    {
                        if (ID_temp / 6 == Clan_ID_e) DrawHint(cal_temp - 97, 8 - _row);
                        break;
                    }
                }
                for (int cal_temp = _calumn - 1; ; cal_temp--)
                {
                    //Left
                    int ID_temp = IDat_II(cal_temp - 97, 8 - _row);
                    if (ID_temp == 0) DrawHint(cal_temp - 97, 8 - _row);
                    else
                    {
                        if (ID_temp / 6 == Clan_ID_e) DrawHint(cal_temp - 97, 8 - _row);
                        break;
                    }
                }
                for (int row_temp = _row + 1; ; row_temp++)
                {
                    //Up
                    int ID_temp = IDat_CI(_calumn, row_temp);
                    if (ID_temp == 0) DrawHint(_calumn, row_temp);
                    else
                    {
                        if (ID_temp / 6 == Clan_ID_e) DrawHint(_calumn, row_temp);
                        break;
                    }
                }
                for (int row_temp = _row - 1; ; row_temp--)
                {
                    //Down
                    int ID_temp = IDat_CI(_calumn, row_temp);
                    if (ID_temp == 0) DrawHint(_calumn, row_temp);
                    else
                    {
                        if (ID_temp / 6 == Clan_ID_e) DrawHint(_calumn, row_temp);
                        break;
                    }
                }

                //Bishop Mode
                //Top-Left
                for (int cal_temp = _calumn - 1, row_temp = _row + 1; ; cal_temp--, row_temp++)
                {
                    int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                    if (ID_temp == 0) DrawHint(cal_temp - 97, 8 - row_temp);
                    else
                    {
                        if (ID_temp / 6 == Clan_ID_e) DrawHint(cal_temp - 97, 8 - row_temp);
                        break;
                    }

                }//Top-Right
                for (int cal_temp = _calumn + 1, row_temp = _row + 1; ; cal_temp++, row_temp++)
                {
                    int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                    if (ID_temp == 0) DrawHint(cal_temp - 97, 8 - row_temp);
                    else
                    {
                        if (ID_temp / 6 == Clan_ID_e) DrawHint(cal_temp - 97, 8 - row_temp);
                        break;
                    }

                }
                //Bottum-Left
                for (int cal_temp = _calumn - 1, row_temp = _row - 1; ; cal_temp--, row_temp--)
                {
                    int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                    if (ID_temp == 0) DrawHint(cal_temp - 97, 8 - row_temp);
                    else
                    {
                        if (ID_temp / 6 == Clan_ID_e) DrawHint(cal_temp - 97, 8 - row_temp);
                        break;
                    }

                }
                //Bottom-right
                for (int cal_temp = _calumn + 1, row_temp = _row - 1; ; cal_temp++, row_temp--)
                {
                    int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                    if (ID_temp == 0) DrawHint(cal_temp - 97, 8 - row_temp);
                    else
                    {
                        if (ID_temp / 6 == Clan_ID_e) DrawHint(cal_temp - 97, 8 - row_temp);
                        break;
                    }

                }
            }
            public override bool HasAnyStep()
            {//Rock Mode
                for (int cal_temp = _calumn + 1; ; cal_temp++)
                {
                    //Right
                    int ID_temp = IDat_II(cal_temp - 97, 8 - _row);
                    if (ID_temp == 0)
                    {
                        if (Valid_Test(_calumn, _row, (char)(cal_temp), _row)) return true;
                    }
                    else
                    {
                        if (ID_temp / 6 == Clan_ID_e)
                            if (Valid_Test(_calumn, _row, (char)(cal_temp), _row)) return true;
                        break;
                    }
                }
                for (int cal_temp = _calumn - 1; ; cal_temp--)
                {
                    //Left
                    int ID_temp = IDat_II(cal_temp - 97, 8 - _row);
                    if (ID_temp == 0)
                    {
                        if (Valid_Test(_calumn, _row, (char)(cal_temp), _row)) return true;
                    }
                    else
                    {
                        if (ID_temp / 6 == Clan_ID_e)
                            if (Valid_Test(_calumn, _row, (char)(cal_temp), _row)) return true;
                        break;
                    }
                }
                for (int row_temp = _row + 1; ; row_temp++)
                {
                    //Up
                    int ID_temp = IDat_CI(_calumn, row_temp);
                    if (ID_temp == 0)
                    {
                        if (Valid_Test(_calumn, _row, _calumn, row_temp)) return true;
                    }
                    else
                    {
                        if (ID_temp / 6 == Clan_ID_e)
                            if (Valid_Test(_calumn, _row, _calumn, row_temp)) return true;
                        break;
                    }
                }
                for (int row_temp = _row - 1; ; row_temp--)
                {
                    //Down
                    int ID_temp = IDat_CI(_calumn, row_temp);
                    if (ID_temp == 0)
                    {
                        if (Valid_Test(_calumn, _row, _calumn, row_temp)) return true;
                    }
                    else
                    {
                        if (ID_temp / 6 == Clan_ID_e)
                            if (Valid_Test(_calumn, _row, _calumn, row_temp)) return true;
                        break;
                    }
                }

                //Bishop Mode
                //Top-Left
                for (int cal_temp = _calumn - 1, row_temp = _row + 1; ; cal_temp--, row_temp++)
                {
                    int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                    if (ID_temp == 0)
                    {
                        if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                    }
                    else
                    {
                        if (ID_temp / 6 == Clan_ID_e)
                            if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                        break;
                    }
                }
                //Top-Right
                for (int cal_temp = _calumn + 1, row_temp = _row + 1; ; cal_temp++, row_temp++)
                {
                    int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                    if (ID_temp == 0)
                    {
                        if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                    }
                    else
                    {
                        if (ID_temp / 6 == Clan_ID_e)
                            if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                        break;
                    }

                }
                //Bottum-Left
                for (int cal_temp = _calumn - 1, row_temp = _row - 1; ; cal_temp--, row_temp--)
                {
                    int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                    if (ID_temp == 0)
                    {
                        if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                    }
                    else
                    {
                        if (ID_temp / 6 == Clan_ID_e)
                            if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                        break;
                    }

                }
                //Bottom-right
                for (int cal_temp = _calumn + 1, row_temp = _row - 1; ; cal_temp++, row_temp--)
                {
                    int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                    if (ID_temp == 0)
                    {
                        if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                    }
                    else
                    {
                        if (ID_temp / 6 == Clan_ID_e)
                            if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                        break;
                    }
                }
                //
                return false;
            }
        }
        public class King : Chess
        {
            public King(Clan clanarg)
                : base(clanarg)
            {
                switch (this.clan)
                {
                    case Clan.White:
                        this.Image = Chessing_UI.Properties.Resources.wK;
                        this.ID = 11;
                        this.Clan_ID = 1;
                        this.Clan_ID_e = 2;
                        break;
                    case Clan.Black:
                        this.Image = Chessing_UI.Properties.Resources.bK;
                        this.ID = 17;
                        this.Clan_ID = 2;
                        this.Clan_ID_e = 1;
                        break;
                }
            }
            public override bool CanMoveTo(char _calumnarg, int _rowarg)
            {
                int Target_ID = IDat_CI(_calumnarg, _rowarg);
                if (Target_ID == 60229 || (Target_ID / 6) == Clan_ID) return false;           //Out of range or Target is ally 

                if ((Math.Abs(_calumnarg - _calumn) < 2) && (Math.Abs(_rowarg - _row) < 2))
                {
                    //normal step
                    return true;
                }

                if ((_rowarg != _row) || (this.Moved)) return false;

                if (_calumnarg == 'c' && IDat_CI('c', _row) == 0)
                {
                    //Castle
                    if (IDat_CI('b', _row) != 0) return false;
                    else if (IDat_CI('d', _row) != 0) return false;

                    switch (Clan_ID)
                    {
                        case 1:
                            //White
                            WhoGuarding = Shiro.R1;  //White's Rock 1(a-1)
                            if (WhoGuarding.Moved) return false;    //Guarian can't be moved

                            //Check ('e',1)
                            for (int cal_temp = 102; cal_temp <= 104; cal_temp++)
                            {
                                //Right
                                int ID_temp = IDat_II(cal_temp - 97, 7);
                                if (ID_temp == 0) continue;

                                if ((cal_temp == 102) && (ID_temp == 17)) return false;     //Black King attacking
                                if (ID_temp == 14 || ID_temp == 16) return false;           //Black Rock n Queen attacking
                                break;
                            }
                            for (int cal_temp = 102, row_temp = 2; cal_temp <= 104; cal_temp++, row_temp++)
                            {
                                //Top-Right
                                int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                                if (ID_temp == 0) continue;

                                if (cal_temp == 102)
                                    if (ID_temp == 12 || ID_temp == 17) return false;       //Black Pawn n King attacking
                                if (ID_temp == 15 || ID_temp == 16) return false;           //Black Bishop n Queen attacking
                                break;
                            }
                            for (int row_temp = 2; row_temp <= 8; row_temp++)
                            {
                                //Top
                                int ID_temp = IDat_CI('e', row_temp);
                                if (ID_temp == 0) continue;

                                if ((row_temp == 2) && (ID_temp == 17)) return false;       //Black King attacking
                                if (ID_temp == 14 || ID_temp == 16) return false;           //Black Rock n Queen attacking
                                break;
                            }
                            for (int cal_temp = 100, row_temp = 2; cal_temp >= 97; cal_temp--, row_temp++)
                            {
                                //Top-Left
                                int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                                if (ID_temp == 0) continue;

                                if (cal_temp == 100)
                                    if (ID_temp == 12 || ID_temp == 17) return false;       //Black Pawn n King attacking
                                if (ID_temp == 15 || ID_temp == 16) return false;           //Black Bishop n Queen attacking
                                break;
                            }
                            //Black Knight attacking
                            if (IDat_CI('g', 2) == 13) return false;
                            else if (IDat_CI('f', 3) == 13) return false;
                            else if (IDat_CI('d', 3) == 13) return false;
                            else if (IDat_CI('c', 2) == 13) return false;

                            //Check ('d',1)
                            for (int cal_temp = 101, row_temp = 2; cal_temp <= 104; cal_temp++, row_temp++)
                            {
                                //Top-Right
                                int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                                if (ID_temp == 0) continue;

                                if (cal_temp == 101)
                                    if (ID_temp == 12 || ID_temp == 17) return false;       //Black Pawn n King attacking
                                if (ID_temp == 15 || ID_temp == 16) return false;           //Black Bishop n Queen attacking
                                break;
                            }
                            for (int row_temp = 2; row_temp <= 8; row_temp++)
                            {
                                //Top
                                int ID_temp = IDat_CI('d', row_temp);
                                if (ID_temp == 0) continue;

                                if ((row_temp == 2) && (ID_temp == 17)) return false;       //Black King attacking
                                if (ID_temp == 14 || ID_temp == 16) return false;           //Black Rock n Queen attacking
                                break;
                            }
                            for (int cal_temp = 99, row_temp = 2; cal_temp >= 97; cal_temp--, row_temp++)
                            {
                                //Top-Left
                                int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                                if (ID_temp == 0) continue;

                                if (cal_temp == 99)
                                    if (ID_temp == 12 || ID_temp == 17) return false;       //Black Pawn n King attacking
                                if (ID_temp == 15 || ID_temp == 16) return false;           //Black Bishop n Queen attacking
                                break;
                            }
                            //Black Knight attacking
                            if (IDat_CI('f', 2) == 13) return false;
                            else if (IDat_CI('e', 3) == 13) return false;
                            else if (IDat_CI('c', 3) == 13) return false;
                            else if (IDat_CI('b', 2) == 13) return false;

                            //Check ('c',1)
                            for (int cal_temp = 100, row_temp = 2; cal_temp <= 104; cal_temp++, row_temp++)
                            {
                                //Top-Right
                                int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                                if (ID_temp == 0) continue;

                                if (cal_temp == 100)
                                    if (ID_temp == 12 || ID_temp == 17) return false;       //Black Pawn n King attacking
                                if (ID_temp == 15 || ID_temp == 16) return false;           //Black Bishop n Queen attacking
                                break;
                            }
                            for (int row_temp = 2; row_temp <= 8; row_temp++)
                            {
                                //Top
                                int ID_temp = IDat_CI('c', row_temp);
                                if (ID_temp == 0) continue;

                                if ((row_temp == 2) && (ID_temp == 17)) return false;       //Black King attacking
                                if (ID_temp == 14 || ID_temp == 16) return false;           //Black Rock n Queen attacking
                                break;
                            }
                            for (int cal_temp = 98, row_temp = 2; cal_temp >= 97; cal_temp--, row_temp++)
                            {
                                //Top-Left
                                int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                                if (ID_temp == 0) continue;

                                if (cal_temp == 98)
                                    if (ID_temp == 12 || ID_temp == 17) return false;       //Black Pawn n King attacking
                                if (ID_temp == 15 || ID_temp == 16) return false;           //Black Bishop n Queen attacking
                                break;
                            }
                            //Black Knight attacking
                            if (IDat_CI('e', 2) == 13) return false;
                            else if (IDat_CI('d', 3) == 13) return false;
                            else if (IDat_CI('b', 3) == 13) return false;
                            else if (IDat_CI('a', 2) == 13) return false;

                            return true;
                        case 2:
                            //Black
                            WhoGuarding = Kuro.R1;  //Black's Rock 1(a-8)
                            if (WhoGuarding.Moved) return false;    //Guarian can't be moved

                            //Check ('e',8)
                            for (int cal_temp = 102; cal_temp <= 104; cal_temp++)
                            {
                                //Right
                                int ID_temp = IDat_II(cal_temp - 97, 0);
                                if (ID_temp == 0) continue;

                                if ((cal_temp == 102) && (ID_temp == 11)) return false;     //White King attacking
                                if (ID_temp == 8 || ID_temp == 10) return false;           //White Rock n Queen attacking
                                break;
                            }
                            for (int cal_temp = 102, row_temp = 7; cal_temp <= 104; cal_temp++, row_temp--)
                            {
                                //Bottom-Right
                                int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                                if (ID_temp == 0) continue;

                                if (cal_temp == 102)
                                    if (ID_temp == 6 || ID_temp == 11) return false;       //White Pawn n King attacking
                                if (ID_temp == 9 || ID_temp == 10) return false;           //White Bishop n Queen attacking
                                break;
                            }
                            for (int row_temp = 7; row_temp >= 1; row_temp--)
                            {
                                //Bottom
                                int ID_temp = IDat_CI('e', row_temp);
                                if (ID_temp == 0) continue;

                                if ((row_temp == 7) && (ID_temp == 11)) return false;       //White King attacking
                                if (ID_temp == 8 || ID_temp == 10) return false;           //White Rock n Queen attacking
                                break;
                            }
                            for (int cal_temp = 100, row_temp = 7; cal_temp >= 97; cal_temp--, row_temp--)
                            {
                                //Bottom-Left
                                int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                                if (ID_temp == 0) continue;

                                if (cal_temp == 100)
                                    if (ID_temp == 6 || ID_temp == 11) return false;       //White Pawn n King attacking
                                if (ID_temp == 9 || ID_temp == 10) return false;           //White Bishop n Queen attacking
                                break;
                            }
                            //White Knight attacking
                            if (IDat_CI('g', 7) == 7) return false;
                            else if (IDat_CI('f', 6) == 7) return false;
                            else if (IDat_CI('d', 6) == 7) return false;
                            else if (IDat_CI('c', 7) == 7) return false;

                            //Check ('d',8)
                            for (int cal_temp = 101, row_temp = 7; cal_temp <= 104; cal_temp++, row_temp--)
                            {
                                //Bottom-Right
                                int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                                if (ID_temp == 0) continue;

                                if (cal_temp == 101)
                                    if (ID_temp == 6 || ID_temp == 11) return false;       //White Pawn n King attacking
                                if (ID_temp == 9 || ID_temp == 10) return false;           //White Bishop n Queen attacking
                                break;
                            }
                            for (int row_temp = 7; row_temp >= 1; row_temp--)
                            {
                                //Bottom
                                int ID_temp = IDat_CI('d', row_temp);
                                if (ID_temp == 0) continue;

                                if ((row_temp == 7) && (ID_temp == 11)) return false;       //White King attacking
                                if (ID_temp == 8 || ID_temp == 10) return false;           //White Rock n Queen attacking
                                break;
                            }
                            for (int cal_temp = 99, row_temp = 7; cal_temp >= 97; cal_temp--, row_temp--)
                            {
                                //Bottom-Left
                                int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                                if (ID_temp == 0) continue;

                                if (cal_temp == 99)
                                    if (ID_temp == 6 || ID_temp == 11) return false;       //White Pawn n King attacking
                                if (ID_temp == 9 || ID_temp == 10) return false;           //White Bishop n Queen attacking
                                break;
                            }
                            //White Knight attacking
                            if (IDat_CI('f', 7) == 7) return false;
                            else if (IDat_CI('e', 6) == 7) return false;
                            else if (IDat_CI('c', 6) == 7) return false;
                            else if (IDat_CI('b', 7) == 7) return false;

                            //Check ('c',8)
                            for (int cal_temp = 100, row_temp = 7; cal_temp <= 104; cal_temp++, row_temp--)
                            {
                                //Bottom-Right
                                int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                                if (ID_temp == 0) continue;

                                if (cal_temp == 100)
                                    if (ID_temp == 6 || ID_temp == 11) return false;       //White Pawn n King attacking
                                if (ID_temp == 9 || ID_temp == 10) return false;           //White Bishop n Queen attacking
                                break;
                            }
                            for (int row_temp = 7; row_temp >= 1; row_temp--)
                            {
                                //Bottom
                                int ID_temp = IDat_CI('c', row_temp);
                                if (ID_temp == 0) continue;

                                if ((row_temp == 7) && (ID_temp == 11)) return false;       //White King attacking
                                if (ID_temp == 8 || ID_temp == 10) return false;           //White Rock n Queen attacking
                                break;
                            }
                            for (int cal_temp = 98, row_temp = 7; cal_temp >= 97; cal_temp--, row_temp--)
                            {
                                //Bottom-Left
                                int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                                if (ID_temp == 0) continue;

                                if (cal_temp == 98)
                                    if (ID_temp == 6 || ID_temp == 11) return false;       //White Pawn n King attacking
                                if (ID_temp == 9 || ID_temp == 10) return false;           //White Bishop n Queen attacking
                                break;
                            }
                            //White Knight attacking
                            if (IDat_CI('e', 7) == 7) return false;
                            else if (IDat_CI('d', 6) == 7) return false;
                            else if (IDat_CI('b', 6) == 7) return false;
                            else if (IDat_CI('a', 7) == 7) return false;

                            return true;
                    }
                }
                else if (_calumnarg == 'g' && IDat_CI('g', _row) == 0)
                {
                    //Castle
                    if (IDat_CI('f', _row) != 0) return false;

                    switch (Clan_ID)
                    {
                        case 1:
                            //White
                            WhoGuarding = Shiro.R2;  //White's Rock 2(h-1)
                            if (WhoGuarding.Moved) return false;    //Guarian can't be moved

                            //Check ('e',1)
                            for (int cal_temp = 100; cal_temp >= 97; cal_temp--)
                            {
                                //Left
                                int ID_temp = IDat_II(cal_temp - 97, 7);
                                if (ID_temp == 0) continue;

                                if ((cal_temp == 100) && (ID_temp == 17)) return false;     //Black King attacking
                                if (ID_temp == 14 || ID_temp == 16) return false;           //Black Rock n Queen attacking
                                break;
                            }
                            for (int cal_temp = 102, row_temp = 2; cal_temp <= 104; cal_temp++, row_temp++)
                            {
                                //Top-Right
                                int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                                if (ID_temp == 0) continue;

                                if (cal_temp == 102)
                                    if (ID_temp == 12 || ID_temp == 17) return false;       //Black Pawn n King attacking
                                if (ID_temp == 15 || ID_temp == 16) return false;           //Black Bishop n Queen attacking
                                break;
                            }
                            for (int row_temp = 2; row_temp <= 8; row_temp++)
                            {
                                //Top
                                int ID_temp = IDat_CI('e', row_temp);
                                if (ID_temp == 0) continue;

                                if ((row_temp == 2) && (ID_temp == 17)) return false;       //Black King attacking
                                if (ID_temp == 14 || ID_temp == 16) return false;           //Black Rock n Queen attacking
                                break;
                            }
                            for (int cal_temp = 100, row_temp = 2; cal_temp >= 97; cal_temp--, row_temp++)
                            {
                                //Top-Left
                                int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                                if (ID_temp == 0) continue;

                                if (cal_temp == 100)
                                    if (ID_temp == 12 || ID_temp == 17) return false;       //Black Pawn n King attacking
                                if (ID_temp == 15 || ID_temp == 16) return false;           //Black Bishop n Queen attacking
                                break;
                            }
                            //Black Knight attacking
                            if (IDat_CI('g', 2) == 13) return false;
                            else if (IDat_CI('f', 3) == 13) return false;
                            else if (IDat_CI('d', 3) == 13) return false;
                            else if (IDat_CI('c', 2) == 13) return false;

                            //Check ('f',1)
                            for (int cal_temp = 103, row_temp = 2; cal_temp <= 104; cal_temp++, row_temp++)
                            {
                                //Top-Right
                                int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                                if (ID_temp == 0) continue;

                                if (cal_temp == 103)
                                    if (ID_temp == 12 || ID_temp == 17) return false;       //Black Pawn n King attacking
                                if (ID_temp == 15 || ID_temp == 16) return false;           //Black Bishop n Queen attacking
                                break;
                            }
                            for (int row_temp = 2; row_temp <= 8; row_temp++)
                            {
                                //Top
                                int ID_temp = IDat_CI('f', row_temp);
                                if (ID_temp == 0) continue;

                                if ((row_temp == 2) && (ID_temp == 17)) return false;       //Black King attacking
                                if (ID_temp == 14 || ID_temp == 16) return false;           //Black Rock n Queen attacking
                                break;
                            }
                            for (int cal_temp = 101, row_temp = 2; cal_temp >= 97; cal_temp--, row_temp++)
                            {
                                //Top-Left
                                int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                                if (ID_temp == 0) continue;

                                if (cal_temp == 101)
                                    if (ID_temp == 12 || ID_temp == 17) return false;       //Black Pawn n King attacking
                                if (ID_temp == 15 || ID_temp == 16) return false;           //Black Bishop n Queen attacking
                                break;
                            }
                            //Black Knight attacking
                            if (IDat_CI('h', 2) == 13) return false;
                            else if (IDat_CI('g', 3) == 13) return false;
                            else if (IDat_CI('e', 3) == 13) return false;
                            else if (IDat_CI('d', 2) == 13) return false;

                            //Check ('g',1)
                            int ID_temp_kai = IDat_CI('h', 2);
                            if (ID_temp_kai == 12 || ID_temp_kai == 17) return false;       //Black Pawn n King attacking
                            if (ID_temp_kai == 15 || ID_temp_kai == 16) return false;       //Black Bishop n Queen attacking

                            for (int row_temp = 2; row_temp <= 8; row_temp++)
                            {
                                //Top
                                int ID_temp = IDat_CI('g', row_temp);
                                if (ID_temp == 0) continue;

                                if ((row_temp == 2) && (ID_temp == 17)) return false;       //Black King attacking
                                if (ID_temp == 14 || ID_temp == 16) return false;           //Black Rock n Queen attacking
                                break;
                            }
                            for (int cal_temp = 102, row_temp = 2; cal_temp >= 97; cal_temp--, row_temp++)
                            {
                                //Top-Left
                                int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                                if (ID_temp == 0) continue;

                                if (cal_temp == 102)
                                    if (ID_temp == 12 || ID_temp == 17) return false;       //Black Pawn n King attacking
                                if (ID_temp == 15 || ID_temp == 16) return false;           //Black Bishop n Queen attacking
                                break;
                            }
                            //Black Knight attacking
                            if (IDat_CI('h', 3) == 13) return false;
                            else if (IDat_CI('f', 3) == 13) return false;
                            else if (IDat_CI('e', 2) == 13) return false;

                            return true;
                        case 2:
                            //Black
                            WhoGuarding = Kuro.R2;  //Black's Rock 1(h-8)
                            if (WhoGuarding.Moved) return false;    //Guarian can't be moved

                            //Check ('e',8)
                            for (int cal_temp = 100; cal_temp >= 97; cal_temp--)
                            {
                                //Left
                                int ID_temp = IDat_II(cal_temp - 97, 0);
                                if (ID_temp == 0) continue;

                                if ((cal_temp == 100) && (ID_temp == 11)) return false;     //White King attacking
                                if (ID_temp == 8 || ID_temp == 10) return false;           //White Rock n Queen attacking
                                break;
                            }
                            for (int cal_temp = 102, row_temp = 7; cal_temp <= 104; cal_temp++, row_temp--)
                            {
                                //Bottom-Right
                                int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                                if (ID_temp == 0) continue;

                                if (cal_temp == 102)
                                    if (ID_temp == 6 || ID_temp == 11) return false;       //White Pawn n King attacking
                                if (ID_temp == 9 || ID_temp == 10) return false;           //White Bishop n Queen attacking
                                break;
                            }
                            for (int row_temp = 7; row_temp >= 1; row_temp--)
                            {
                                //Bottom
                                int ID_temp = IDat_CI('e', row_temp);
                                if (ID_temp == 0) continue;

                                if ((row_temp == 7) && (ID_temp == 11)) return false;       //White King attacking
                                if (ID_temp == 8 || ID_temp == 10) return false;            //White Rock n Queen attacking
                                break;
                            }
                            for (int cal_temp = 100, row_temp = 7; cal_temp >= 97; cal_temp--, row_temp--)
                            {
                                //Bottom-Left
                                int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                                if (ID_temp == 0) continue;

                                if (cal_temp == 100)
                                    if (ID_temp == 6 || ID_temp == 11) return false;       //White Pawn n King attacking
                                if (ID_temp == 9 || ID_temp == 10) return false;           //White Bishop n Queen attacking
                                break;
                            }
                            //White Knight attacking
                            if (IDat_CI('g', 7) == 7) return false;
                            else if (IDat_CI('f', 6) == 7) return false;
                            else if (IDat_CI('d', 6) == 7) return false;
                            else if (IDat_CI('c', 7) == 7) return false;

                            //Check ('f',8)
                            for (int cal_temp = 103, row_temp = 7; cal_temp <= 104; cal_temp++, row_temp--)
                            {
                                //Bottom-Right
                                int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                                if (ID_temp == 0) continue;

                                if (cal_temp == 103)
                                    if (ID_temp == 6 || ID_temp == 11) return false;       //White Pawn n King attacking
                                if (ID_temp == 9 || ID_temp == 10) return false;           //White Bishop n Queen attacking
                                break;
                            }
                            for (int row_temp = 7; row_temp >= 1; row_temp--)
                            {
                                //Bottom
                                int ID_temp = IDat_CI('f', row_temp);
                                if (ID_temp == 0) continue;

                                if ((row_temp == 7) && (ID_temp == 11)) return false;       //White King attacking
                                if (ID_temp == 8 || ID_temp == 10) return false;            //White Rock n Queen attacking
                                break;
                            }
                            for (int cal_temp = 101, row_temp = 7; cal_temp >= 97; cal_temp--, row_temp--)
                            {
                                //Bottom-Left
                                int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                                if (ID_temp == 0) continue;

                                if (cal_temp == 101)
                                    if (ID_temp == 6 || ID_temp == 11) return false;       //White Pawn n King attacking
                                if (ID_temp == 9 || ID_temp == 10) return false;           //White Bishop n Queen attacking
                                break;
                            }
                            //White Knight attacking
                            if (IDat_CI('h', 7) == 7) return false;
                            else if (IDat_CI('g', 6) == 7) return false;
                            else if (IDat_CI('e', 6) == 7) return false;
                            else if (IDat_CI('d', 7) == 7) return false;

                            //Check ('g',8)
                            int ID_temp_gai = IDat_CI('h', 7);
                            if (ID_temp_gai == 6 || ID_temp_gai == 11) return false;       //White Pawn n King attacking
                            if (ID_temp_gai == 9 || ID_temp_gai == 10) return false;       //White Bishop n Queen attacking

                            for (int row_temp = 7; row_temp >= 1; row_temp--)
                            {
                                //Bottom
                                int ID_temp = IDat_CI('g', row_temp);
                                if (ID_temp == 0) continue;

                                if ((row_temp == 7) && (ID_temp == 11)) return false;       //White King attacking
                                if (ID_temp == 8 || ID_temp == 10) return false;            //White Rock n Queen attacking
                                break;
                            }
                            for (int cal_temp = 102, row_temp = 7; cal_temp >= 97; cal_temp--, row_temp--)
                            {
                                //Bottom-Left
                                int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                                if (ID_temp == 0) continue;

                                if (cal_temp == 102)
                                    if (ID_temp == 6 || ID_temp == 11) return false;       //White Pawn n King attacking
                                if (ID_temp == 9 || ID_temp == 10) return false;           //White Bishop n Queen attacking
                                break;
                            }
                            //White Knight attacking
                            if (IDat_CI('h', 6) == 7) return false;
                            else if (IDat_CI('f', 6) == 7) return false;
                            else if (IDat_CI('e', 7) == 7) return false;

                            return true;
                    }
                }
                return false;
            }
            public override void Hint()
            {
                //Normal step
                for (int cal_temp = _calumn - 1; cal_temp - _calumn < 2; cal_temp++)
                    for (int row_temp = _row + 1; row_temp - _row > -2; row_temp--){
                        int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                        if (ID_temp == 0 || ((ID_temp / 6) == Clan_ID_e)) DrawHint(cal_temp - 97, 8 - row_temp);
                    }

                //Castling
                if (this.Moved) return;
                if (this.CanMoveTo('c', _row)) DrawHint('c', _row);
                if (this.CanMoveTo('g', _row)) DrawHint('g', _row);
            }
            public override bool HasAnyStep()
            {
                //Normal step
                for (int cal_temp = _calumn - 1; cal_temp - _calumn < 2; cal_temp++)
                    for (int row_temp = _row + 1; row_temp - _row > -2; row_temp--){
                        int ID_temp = IDat_II(cal_temp - 97, 8 - row_temp);
                        if (ID_temp == 0 || ((ID_temp / 6) == Clan_ID_e))
                            if (Valid_Test(_calumn, _row, (char)cal_temp, row_temp)) return true;
                    }

                //Castling
                if (this.Moved) return false;
                if (this.CanMoveTo('c', _row)) return true;
                if (this.CanMoveTo('g', _row)) return true;

                return false;
            }
        }
        public class Player
        {
            private Chess.Clan _clan;

            public Pawn P1, P2, P3, P4, P5, P6, P7, P8;
            public Knight N1, N2;
            public Rock R1, R2;
            public Bishop B1, B2;
            public Queen Q;
            public King K;
            public Chess[] Units;

            public Player(Chess.Clan ClanArg)
            {
                _clan = ClanArg;
                Units = new Chess[]{
                P1=new Pawn(_clan),
                P2=new Pawn(_clan),
                P3=new Pawn(_clan),
                P4=new Pawn(_clan),
                P5=new Pawn(_clan),
                P6=new Pawn(_clan),
                P7=new Pawn(_clan),
                P8=new Pawn(_clan),
                N1=new Knight(_clan),
                N2=new Knight(_clan),
                R1=new Rock(_clan),
                R2=new Rock(_clan),
                B1=new Bishop(_clan),
                B2=new Bishop(_clan),
                Q=new Queen(_clan),
                K=new King(_clan)
            };
                foreach (Chess unit in Units)
                {
                    unit.MouseClick += Picked;
                }
            }
            public Chess.Clan clan
            {
                get { return _clan; }
            }
            public void Stand()
            {
                foreach (Chess unit in Units) { unit.Moved = false; }
                switch (_clan)
                {
                    case Chess.Clan.White:
                        P1.ID = 6; P1.Image = Chessing_UI.Properties.Resources.wP; P1.SetTo('a', 2);
                        P2.ID = 6; P2.Image = Chessing_UI.Properties.Resources.wP; P2.SetTo('b', 2);
                        P3.ID = 6; P3.Image = Chessing_UI.Properties.Resources.wP; P3.SetTo('c', 2);
                        P4.ID = 6; P4.Image = Chessing_UI.Properties.Resources.wP; P4.SetTo('d', 2);
                        P5.ID = 6; P5.Image = Chessing_UI.Properties.Resources.wP; P5.SetTo('e', 2);
                        P6.ID = 6; P6.Image = Chessing_UI.Properties.Resources.wP; P6.SetTo('f', 2);
                        P7.ID = 6; P7.Image = Chessing_UI.Properties.Resources.wP; P7.SetTo('g', 2);
                        P8.ID = 6; P8.Image = Chessing_UI.Properties.Resources.wP; P8.SetTo('h', 2);
                        N1.ID = 7; N1.SetTo('b', 1);
                        N2.ID = 7; N2.SetTo('g', 1);
                        R1.ID = 8; R1.SetTo('a', 1);
                        R2.ID = 8; R2.SetTo('h', 1);
                        B1.ID = 9; B1.SetTo('c', 1);
                        B2.ID = 9; B2.SetTo('f', 1);
                        Q.ID = 10; Q.SetTo('d', 1);
                        K.ID = 11; K.SetTo('e', 1);
                        break;
                    case Chess.Clan.Black:
                        P1.ID = 12; P1.Image = Chessing_UI.Properties.Resources.bP; P1.SetTo('a', 7);
                        P2.ID = 12; P2.Image = Chessing_UI.Properties.Resources.bP; P2.SetTo('b', 7);
                        P3.ID = 12; P3.Image = Chessing_UI.Properties.Resources.bP; P3.SetTo('c', 7);
                        P4.ID = 12; P4.Image = Chessing_UI.Properties.Resources.bP; P4.SetTo('d', 7);
                        P5.ID = 12; P5.Image = Chessing_UI.Properties.Resources.bP; P5.SetTo('e', 7);
                        P6.ID = 12; P6.Image = Chessing_UI.Properties.Resources.bP; P6.SetTo('f', 7);
                        P7.ID = 12; P7.Image = Chessing_UI.Properties.Resources.bP; P7.SetTo('g', 7);
                        P8.ID = 12; P8.Image = Chessing_UI.Properties.Resources.bP; P8.SetTo('h', 7);
                        N1.ID = 13; N1.SetTo('b', 8);
                        N2.ID = 13; N2.SetTo('g', 8);
                        R1.ID = 14; R1.SetTo('a', 8);
                        R2.ID = 14; R2.SetTo('h', 8);
                        B1.ID = 15; B1.SetTo('c', 8);
                        B2.ID = 15; B2.SetTo('f', 8);
                        Q.ID = 16; Q.SetTo('d', 8);
                        K.ID = 17; K.SetTo('e', 8);
                        break;
                }
            }
        }

        public static Player Shiro, Kuro;
        private static int[][] IDMatrix = new int[8][] { new int[8], new int[8], new int[8], new int[8], new int[8], new int[8], new int[8], new int[8] };

        private class MyMenuTripColorTable : ProfessionalColorTable
        {
            //↓框線設定透明
            public override Color MenuItemBorder
            {
                get
                {
                    return Color.FromArgb(0, 0, 0, 0);
                }
            }
            //↓經過選項時的顏色設定(經過Game以及Help的顏色)
            public override Color MenuItemSelectedGradientBegin
            {
                get
                {
                    return Color.FromArgb(255, 192, 175, 114);
                }
            }
            public override Color MenuItemSelectedGradientEnd
            {
                get
                {
                    return Color.FromArgb(255, 192, 175, 114);
                }
            }
            //↓選取選項時的顏色設定(表單維持下拉狀態時Game的顏色)
            public override Color MenuItemPressedGradientBegin
            {
                get
                {
                    return Color.RoyalBlue;
                }
            }
            public override Color MenuItemPressedGradientMiddle
            {
                get
                {
                    return Color.Pink;
                }
            }
            public override Color MenuItemPressedGradientEnd
            {
                get
                {
                    return Color.GreenYellow;
                }
            }
            //↓
        }

        private static Bitmap boardPIC = new Bitmap(512, 512);
        public static PictureBox HintMap = new PictureBox();
        public static Bitmap hintMapPIC = new Bitmap(512, 512);
        public static PictureBox TrackMap = new PictureBox();
        public static Bitmap trackMapPIC = new Bitmap(512, 512);

        private static SolidBrush HintBrush = new SolidBrush(Color.FromArgb(71, 132, 15, 237));
        private static SolidBrush TrackBrush = new SolidBrush(Color.FromArgb(101, 192, 21, 88));

        public static List<Chess> CheckingList = new List<Chess>();

        public static Valhalla Valhalla_holy = new Valhalla();
        public static Valhalla Valhalla_inferno = new Valhalla();

        public static int Turn = 1;
        public static string record = "";
        public static HisRecorder Recorder = new HisRecorder();

        //Painful Part
        public static Chess_AI.Chess_AI AI_San = new Chess_AI.Chess_AI();
        public static Chess_AI.Point PT_Start = new Chess_AI.Point();
        public static Chess_AI.Point PT_End = new Chess_AI.Point();
        //

        //Flags
        public static bool En_passantable = false;
        public static Pawn WhoJustStep2;
        public static Rock WhoGuarding;

        public static Chess Chess_Picked = new Chess(Chess.Clan.White);
        public static Chess Chess_Eaten = new Chess(Chess.Clan.White);
        public static bool Picking = false;
        public static bool Checking = false;

        public static bool GameOver = true;

//====================================================== Functions =================================================================================
        
        public Form1()
        {
            InitializeComponent();
            menuStrip1.Renderer = new ToolStripProfessionalRenderer(new MyMenuTripColorTable());
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //Create two players
            Shiro = new Player(Chess.Clan.White);
            Kuro = new Player(Chess.Clan.Black);
            //==================================== Set the board ====================================
            using (Graphics g = Graphics.FromImage(boardPIC))
            {
                Color wh = Color.FromArgb(255, 156, 137, 69);
                SolidBrush bl = new SolidBrush(Color.FromArgb(255, 114, 96, 40));

                g.Clear(wh);
                for (int i = 0; i < 8; i++)
                    for (int j = (i % 2 == 0 ? 1 : 0); j < 8; j += 2)
                    {
                        g.FillRectangle(bl, i * Chess.chess_Size, j * Chess.chess_Size, Chess.chess_Size, Chess.chess_Size);
                    }
            }
            Board.Image = boardPIC;
            //==================================== Cover the trackMap ====================================
            TrackMap.Height = 512;
            TrackMap.Width = 512;
            TrackMap.Location = new Point(0, 0);
            TrackMap.BackColor = Color.Transparent;
            TrackMap.Parent = Board;
            //==================================== Cover the hintMap ====================================
            HintMap.Height = 512;
            HintMap.Width = 512;
            HintMap.Location = new Point(0, 0);
            HintMap.BackColor = Color.Transparent;
            HintMap.Parent = TrackMap;
            HintMap.MouseClick += HintMap_MouseClick;
            //==================================================================================
            
            //Put chesses onto Board
            foreach (Chess unit in Shiro.Units)
            {
                unit.Parent = HintMap;
            }
            foreach (Chess unit in Kuro.Units)
            {
                unit.Parent = HintMap;
            }
            
            //n Sort them
            Shiro.Stand();
            Kuro.Stand();

            Valhalla_holy = Valhalla_Heaven;
            Valhalla_inferno = Valhalla_Hell;
            Recorder = hisRecorder1;
        }

        public static int IDat_CI(char _calumnarg, int _rowarg)
        {
            try { return IDMatrix[8 - _rowarg][_calumnarg - 97]; }
            catch { return 60229; }
        }
        public static int IDat_II(int _calumnarg, int _rowarg)
        {
            try { return IDMatrix[_rowarg][_calumnarg]; }
            catch { return 60229; }
        }
        public static bool Be_Attacking(char CalChk,int RowChk,int _ClanID_E) {
            int ID_Temp=0;
            
            switch (_ClanID_E)
            {
                case 2:
                    //We are White

                    //Pawn Check
                    
                    ID_Temp = IDat_II(CalChk - 98, 7 - RowChk);     //Front-Left 
                    if (ID_Temp == 12 || ID_Temp == 15) return true;    //Black Pawn,Bishop
                    if (ID_Temp == 16 || ID_Temp == 17) return true;    //Black Queen,King
                    
                    ID_Temp = IDat_II(CalChk - 96, 7 - RowChk);     //Front-Right 
                    if (ID_Temp == 12 || ID_Temp == 15) return true;    //Black Pawn,Bishop
                    if (ID_Temp == 16 || ID_Temp == 17) return true;    //Black Queen,King

                    //Knight Check
                    if (CalChk > 97)
                    {
                        //Left
                        if (IDat_II(CalChk - 98, 6 - RowChk) == 13) return true;    //-1,+2
                        if (IDat_II(CalChk - 98, 10 - RowChk) == 13) return true;   //-1,-2
                        if (CalChk > 98)
                        {
                            if (IDat_II(CalChk - 99, 7 - RowChk) == 13) return true;    //-2,+1
                            if (IDat_II(CalChk - 99, 9 - RowChk) == 13) return true;    //-2,-1
                        }
                    }
                    if (CalChk < 104)
                    {
                        //Right
                        if (IDat_II(CalChk - 96, 6 - RowChk) == 13) return true;    //+1,+2
                        if (IDat_II(CalChk - 96, 10 - RowChk) == 13) return true;   //+1,-2
                        if (CalChk < 103)
                        {
                            if (IDat_II(CalChk - 95, 7 - RowChk) == 13) return true;    //+2,+1
                            if (IDat_II(CalChk - 95, 9 - RowChk) == 13) return true;    //+2,-1
                        }
                    }

                    //Rock Check
                    for (int row_temp = RowChk + 1; ; row_temp++) {
                        //Up
                        ID_Temp = IDat_CI(CalChk, row_temp);
                        if (ID_Temp == 0) continue;
                        else if (ID_Temp == 14 || ID_Temp == 16) return true;   //Black Rock,Queen
                        break;
                    }
                    for (int row_temp = RowChk - 1; ; row_temp--)
                    {
                        //Down
                        ID_Temp = IDat_CI(CalChk, row_temp);
                        if (ID_Temp == 0) continue;
                        else if (ID_Temp == 14 || ID_Temp == 16) return true;   //Black Rock,Queen
                        break;
                    }
                    for (int cal_temp = CalChk - 1; ; cal_temp--)
                    {
                        //Left
                        ID_Temp = IDat_II(cal_temp - 97, 8 - RowChk);
                        if (ID_Temp == 0) continue;
                        else if (ID_Temp == 14 || ID_Temp == 16) return true;   //Black Rock,Queen
                        break;
                    }
                    for (int cal_temp = CalChk + 1; ; cal_temp++)
                    {
                        //Right
                        ID_Temp = IDat_II(cal_temp - 97, 8 - RowChk);
                        if (ID_Temp == 0) continue;
                        else if (ID_Temp == 14 || ID_Temp == 16) return true;   //Black Rock,Queen
                        break;
                    }

                    //Bishop Check
                    for (int cal_temp = CalChk - 1, row_temp = RowChk + 1; ; cal_temp--, row_temp++) {
                        //Top-Left
                        ID_Temp = IDat_II(cal_temp - 97, 8 - row_temp);
                        if (ID_Temp == 0) continue;
                        else if (ID_Temp == 15 || ID_Temp == 16) return true;   //Black Bishop,Queen
                        break;    
                    }
                    for (int cal_temp = CalChk + 1, row_temp = RowChk + 1; ; cal_temp++, row_temp++)
                    {
                        //Top-Right
                        ID_Temp = IDat_II(cal_temp - 97, 8 - row_temp);
                        if (ID_Temp == 0) continue;
                        else if (ID_Temp == 15 || ID_Temp == 16) return true;   //Black Bishop,Queen
                        break;
                    }
                    for (int cal_temp = CalChk - 1, row_temp = RowChk - 1; ; cal_temp--, row_temp--)
                    {
                        //Bottom-Left
                        ID_Temp = IDat_II(cal_temp - 97, 8 - row_temp);
                        if (ID_Temp == 0) continue;
                        else if (ID_Temp == 15 || ID_Temp == 16) return true;   //Black Bishop,Queen
                        break;
                    }
                    for (int cal_temp = CalChk + 1, row_temp = RowChk - 1; ; cal_temp++, row_temp--)
                    {
                        //Bottom-Right
                        ID_Temp = IDat_II(cal_temp - 97, 8 - row_temp);
                        if (ID_Temp == 0) continue;
                        else if (ID_Temp == 15 || ID_Temp == 16) return true;   //Black Bishop,Queen
                        break;
                    }

                    //Queen Check == Rock Check + Bishop Check

                    //King Check
                    for (int cal_temp = CalChk - 1;cal_temp-CalChk<2;cal_temp++)
                        for (int row_temp = RowChk + 1; row_temp - RowChk > -2; row_temp--) {
                            ID_Temp = IDat_II(cal_temp - 97, 8 - row_temp);
                            if (ID_Temp == 17) return true;
                        }
                    return false;
                case 1:
                    //We are Black

                    //Pawn Check

                    ID_Temp = IDat_II(CalChk - 98, 9 - RowChk); //Front-Left
                    if (ID_Temp == 6 || ID_Temp == 9) return true;    //White Pawn,Bishop
                    if (ID_Temp == 10 || ID_Temp == 11) return true;    //White Queen,King

                    ID_Temp = IDat_II(CalChk - 96, 9 - RowChk); //Front-Right   
                    if (ID_Temp == 6 || ID_Temp == 9) return true;    //White Pawn,Bishop
                    if (ID_Temp == 10 || ID_Temp == 11) return true;    //White Queen,King

                    //Knight Check
                    if (CalChk > 97)
                    {
                        //Left
                        if (IDat_II(CalChk - 98, 6 - RowChk) == 7) return true;    //-1,+2
                        if (IDat_II(CalChk - 98, 10 - RowChk) == 7) return true;   //-1,-2
                        if (CalChk > 98)
                        {
                            if (IDat_II(CalChk - 99, 7 - RowChk) == 7) return true;    //-2,+1
                            if (IDat_II(CalChk - 99, 9 - RowChk) == 7) return true;    //-2,-1
                        }
                    }
                    if (CalChk < 104)
                    {
                        //Right
                        if (IDat_II(CalChk - 96, 6 - RowChk) == 7) return true;    //+1,+2
                        if (IDat_II(CalChk - 96, 10 - RowChk) == 7) return true;   //+1,-2
                        if (CalChk < 103)
                        {
                            if (IDat_II(CalChk - 95, 7 - RowChk) == 7) return true;    //+2,+1
                            if (IDat_II(CalChk - 95, 9 - RowChk) == 7) return true;    //+2,-1
                        }
                    }

                    //Rock Check
                    for (int row_temp = RowChk + 1; ; row_temp++)
                    {
                        //Up
                        ID_Temp = IDat_CI(CalChk, row_temp);
                        if (ID_Temp == 0) continue;
                        else if (ID_Temp == 8 || ID_Temp == 10) return true;   //White Rock,Queen
                        break;
                    }
                    for (int row_temp = RowChk - 1; ; row_temp--)
                    {
                        //Down
                        ID_Temp = IDat_CI(CalChk, row_temp);
                        if (ID_Temp == 0) continue;
                        else if (ID_Temp == 8 || ID_Temp == 10) return true;   //White Rock,Queen
                        break;
                    }
                    for (int cal_temp = CalChk - 1; ; cal_temp--)
                    {
                        //Left
                        ID_Temp = IDat_II(cal_temp - 97, 8 - RowChk);
                        if (ID_Temp == 0) continue;
                        else if (ID_Temp == 8 || ID_Temp == 10) return true;   //White Rock,Queen
                        break;
                    }
                    for (int cal_temp = CalChk + 1; ; cal_temp++)
                    {
                        //Right
                        ID_Temp = IDat_II(cal_temp - 97, 8 - RowChk);
                        if (ID_Temp == 0) continue;
                        else if (ID_Temp == 8 || ID_Temp == 10) return true;   //White Rock,Queen
                        break;
                    }

                    //Bishop Check
                    for (int cal_temp = CalChk - 1, row_temp = RowChk + 1; ; cal_temp--, row_temp++)
                    {
                        //Top-Left
                        ID_Temp = IDat_II(cal_temp - 97, 8 - row_temp);
                        if (ID_Temp == 0) continue;
                        else if (ID_Temp == 9 || ID_Temp == 10) return true;   //White Bishop,Queen
                        break;
                    }
                    for (int cal_temp = CalChk + 1, row_temp = RowChk + 1; ; cal_temp++, row_temp++)
                    {
                        //Top-Right
                        ID_Temp = IDat_II(cal_temp - 97, 8 - row_temp);
                        if (ID_Temp == 0) continue;
                        else if (ID_Temp == 9 || ID_Temp == 10) return true;   //White Bishop,Queen
                        break;
                    }
                    for (int cal_temp = CalChk - 1, row_temp = RowChk - 1; ; cal_temp--, row_temp--)
                    {
                        //Bottom-Left
                        ID_Temp = IDat_II(cal_temp - 97, 8 - row_temp);
                        if (ID_Temp == 0) continue;
                        else if (ID_Temp == 9 || ID_Temp == 10) return true;   //White Bishop,Queen
                        break;
                    }
                    for (int cal_temp = CalChk + 1, row_temp = RowChk - 1; ; cal_temp++, row_temp--)
                    {
                        //Bottom-Right
                        ID_Temp = IDat_II(cal_temp - 97, 8 - row_temp);
                        if (ID_Temp == 0) continue;
                        else if (ID_Temp == 9 || ID_Temp == 10) return true;   //White Bishop,Queen
                        break;
                    }

                    //Queen Check == Rock Check + Bishop Check

                    //King Check
                    for (int cal_temp = CalChk - 1; cal_temp - CalChk < 2; cal_temp++)
                        for (int row_temp = RowChk + 1; row_temp - RowChk > -2; row_temp--)
                        {
                            ID_Temp = IDat_II(cal_temp - 97, 8 - row_temp);
                            if (ID_Temp == 11) return true;
                        }
                    return false;
                default:
                    return false;
            }
        }

        private static void Picked(object sender, MouseEventArgs e)
        {
            if (!Picking)
            {
                if (((Chess)sender).clan == Chess.Clan.Black) return;
                Chess_Picked = (Chess)sender;
                Picking = true;
                Chess_Picked.Color_Default = Chess.Color_Passed;
                Chess_Picked.BackColor = Chess_Picked.Color_Default;

                //============================ Hint Start ============================
                Chess_Picked.Hint();
                HintMap.Image = hintMapPIC;
                //============================ Hint End ============================
            }
            else
            {
                // Cancel the Hint
                using (Graphics g = Graphics.FromImage(hintMapPIC))
                {
                    g.Clear(Color.Transparent);
                }
                HintMap.Image = hintMapPIC;
                //
                Picking = false;
                Chess_Picked.Color_Default = Color.Transparent;
                Chess_Picked.BackColor = Chess_Picked.Color_Default;

                Chess_Eaten = (Chess)sender;

                if (Chess_Eaten == Chess_Picked) return;
                if (!Chess_Picked.CanMoveTo(Chess_Eaten.calumn, Chess_Eaten.row)) return;
                if (!Valid_Test(Chess_Picked.calumn, Chess_Picked.row, Chess_Eaten.calumn, Chess_Eaten.row)) return;

                //Prepared to set chess to new location
                Checking = false;
                CheckingList.Clear();
                En_passantable = false;

                if (Chess_Picked.ID == 6 && (Chess_Eaten.row == 8))
                    Chess_Picked.EVOlution(); //Check whether any pawn can evolve

                //Repaint the trackMap
                using (Graphics g = Graphics.FromImage(trackMapPIC))
                {
                    g.Clear(Color.Transparent);
                }

                record = Turn.ToString().PadLeft(2, '0') + ":   ";    //Add turn number
                Turn++;

                //Update board's data
                IDMatrix[8 - Chess_Picked.row][Chess_Picked.calumn - 97] = 0;
                DrawTrack(Chess_Picked.calumn, Chess_Picked.row);   //Mark the old location
                record += Chess_Picked.calumn.ToString() + Chess_Picked.row.ToString() + "->";    //Record the step

                PT_Start.X = Chess_Picked.calumn - 97;
                PT_Start.Y = 8 - Chess_Picked.row;


                Chess_Picked.SetTo(Chess_Eaten.calumn, Chess_Eaten.row);
                DrawTrack(Chess_Picked.calumn, Chess_Picked.row);   //Mark the new location
                record += Chess_Picked.calumn.ToString() + Chess_Picked.row.ToString();         //Record the step

                PT_End.X = Chess_Picked.calumn - 97;
                PT_End.Y = 8 - Chess_Picked.row;

                record = String.Format("{0,-15}", record) + "||   ";

                Chess_Eaten.Entomb();
                Chess_Picked.Moved = true;
                TrackMap.Image = trackMapPIC;

                //"Checking of Check" lol
                GameOver = true;
                foreach (Chess Jekll in Shiro.Units)
                    if ((Jekll.ID != 60229) && (Jekll.CanMoveTo(Kuro.K.calumn, Kuro.K.row))) CheckingList.Add(Jekll);
                if (CheckingList.Count > 0) Checking = true;

                foreach(Chess _Chess in Kuro.Units)
                    if ((_Chess.ID != 60229) && (_Chess.HasAnyStep())) {
                        GameOver = false;
                        break;
                    }

                if (GameOver) {
                    if (Checking){
                        //White Checkmate
                        MessageBox.Show("Check mate!! White win!!:D");
                        ReStart();
                        return;
                    }
                    else { 
                        //Stalemate
                        MessageBox.Show("Game Over,Stalemate :((.");
                        ReStart();
                        return;
                    }
                }
                //=========================================== Part of AI Start ================================================
                Checking = false;
                CheckingList.Clear();
                En_passantable = false;
                GameOVer = true;
                
                AI_San.AI_s_Turn(IDMatrix, Chess_Picked.ID, PT_Start, PT_End);
                AI_Move(AI_San.NextStep);
                record += AI_San.NextStep.ElementAt(0).ToString() + AI_San.NextStep.ElementAt(1).ToString() + "->" + AI_San.NextStep.ElementAt(2).ToString() + AI_San.NextStep.ElementAt(3).ToString();
                Recorder.AddRecord(record);
                
                //"Checking of Check" lol
                foreach (Chess Hyde in Kuro.Units)
                    if ((Hyde.ID != 60229) && (Hyde.CanMoveTo(Shiro.K.calumn, Shiro.K.row))) CheckingList.Add(Hyde);
                if (CheckingList.Count > 0) Checking = true;

                foreach (Chess _Chess in Shiro.Units)
                    if ((_Chess.ID != 60229) && (_Chess.HasAnyStep())){
                        GameOver = false;
                        break;
                    }

                if (GameOver){
                    if (Checking){
                        //Black Checkmate
                        MessageBox.Show("Check mate!! Black win!!:D");
                        ReStart();
                        return;
                    }
                    else{
                        //Stalemate
                        MessageBox.Show("Game Over,Stalemate :((.");
                        ReStart();
                        return;
                    }
                }

                foreach (Chess Jekll in Shiro.Units)
                    if ((Jekll.ID != 60229) && (Jekll.CanMoveTo(Kuro.K.calumn, Kuro.K.row))) {
                        //White Checkmate
                        MessageBox.Show("Check mate!! White win!!:D");
                        ReStart();
                        return;
                    }
                //============================================ Part of AI End =================================================
            }
        }
        private void HintMap_MouseClick(object sender, MouseEventArgs e)
        {
            if (!Picking) return;

            // Cancel the Hint
            using (Graphics g = Graphics.FromImage(hintMapPIC))
            {
                g.Clear(Color.Transparent);
            }
            HintMap.Image = hintMapPIC;
            //

            Picking = false;
            Chess_Picked.Color_Default = Color.Transparent;
            Chess_Picked.BackColor = Color.Transparent;

            char Cal_Targeto = (char)(97 + (e.X / Chess.chess_Size));
            int Row_Targeto = 8 - e.Y / Chess.chess_Size;

            if (!Chess_Picked.CanMoveTo(Cal_Targeto, Row_Targeto)) return;
            if (!Valid_Test(Chess_Picked.calumn, Chess_Picked.row, Cal_Targeto, Row_Targeto)) return;

            //Prepared to set chess to new location
            Checking = false;
            CheckingList.Clear();
            En_passantable = false;

            if (Chess_Picked.ID ==6){
                //Check enpassantable n Evolvable

                if (Math.Abs(Row_Targeto - Chess_Picked.row) == 2)
                {
                    En_passantable = true;
                    WhoJustStep2 = (Pawn)Chess_Picked;
                }
                else if (Cal_Targeto != Chess_Picked.calumn)
                {
                    IDMatrix[8 - WhoJustStep2.row][WhoJustStep2.calumn - 97] = 0;
                    WhoJustStep2.Entomb();
                }
                else if (Row_Targeto == 8) Chess_Picked.EVOlution();
            }
            else if ((Chess_Picked.ID ==11) && (Math.Abs(Cal_Targeto - Chess_Picked.calumn) == 2))
            {
                //Check castling
                IDMatrix[8 - WhoGuarding.row][WhoGuarding.calumn - 97] = 0;
                WhoGuarding.Moved = true;
                if (Cal_Targeto == 'c') WhoGuarding.SetTo('d', 1);
                else WhoGuarding.SetTo('f',1);
            }
            
            using (Graphics g = Graphics.FromImage(Form1.trackMapPIC))
            {
                g.Clear(Color.Transparent);
            }
            
            //Update board's data
            record = Turn.ToString().PadLeft(2, '0') + ":   ";    //Add turn number
            Turn++;

            IDMatrix[8 - Chess_Picked.row][Chess_Picked.calumn - 97] = 0;
            DrawTrack(Chess_Picked.calumn, Chess_Picked.row);   //Mark the old location
            record += Chess_Picked.calumn.ToString() + Chess_Picked.row.ToString() + "->";    //Record the step

            PT_Start.X=Chess_Picked.calumn-97;
            PT_Start.Y=8-Chess_Picked.row;

            Chess_Picked.SetTo(Cal_Targeto, Row_Targeto);
            DrawTrack(Chess_Picked.calumn, Chess_Picked.row);   //Mark the new location
            record += Chess_Picked.calumn.ToString() + Chess_Picked.row.ToString();         //Record the step

            PT_End.X=Chess_Picked.calumn-97;
            PT_End.Y=8-Chess_Picked.row;

            record = String.Format("{0,-15}", record) + "||   ";

            Chess_Picked.Moved = true;
            TrackMap.Image = trackMapPIC;

            //"Checking of Check" lol
            GameOver = true;
            foreach (Chess Jekll in Shiro.Units)
                if ((Jekll.ID != 60229) && (Jekll.CanMoveTo(Kuro.K.calumn, Kuro.K.row))) CheckingList.Add(Jekll);
            if (CheckingList.Count > 0) Checking = true;

            foreach (Chess _Chess in Kuro.Units)
                if ((_Chess.ID != 60229) && (_Chess.HasAnyStep()))
                {
                    GameOver = false;
                    break;
                }

            if (GameOver){
                if (Checking){
                    //White Checkmate
                    MessageBox.Show("Check mate!! White win!!:D");
                    ReStart();
                    return;
                }
                else{
                    //Stalemate
                    MessageBox.Show("Game Over,Stalemate :((.");
                    ReStart();
                    return;
                }
            }
            //=========================================== Part of AI Start ================================================
            Checking = false;
            CheckingList.Clear();
            En_passantable = false;
            GameOver = true;
            
            AI_San.AI_s_Turn(IDMatrix, Chess_Picked.ID, PT_Start, PT_End);
            AI_Move(AI_San.NextStep);
            record += AI_San.NextStep.ElementAt(0).ToString() + AI_San.NextStep.ElementAt(1).ToString() + "->" + AI_San.NextStep.ElementAt(2).ToString() + AI_San.NextStep.ElementAt(3).ToString();
            Recorder.AddRecord(record);

            //"Checking of Check" lol
            foreach (Chess Hyde in Kuro.Units)
                if ((Hyde.ID != 60229) && (Hyde.CanMoveTo(Shiro.K.calumn, Shiro.K.row))) CheckingList.Add(Hyde);
            if (CheckingList.Count > 0) Checking = true;

            foreach (Chess _Chess in Shiro.Units)
                if ((_Chess.ID != 60229) && (_Chess.HasAnyStep()))
                {
                    GameOver = false;
                    break;
                }

            if (GameOver){
                if (Checking){
                    //Black Checkmate
                    MessageBox.Show("Check mate!! Black win!!:D");
                    ReStart();
                    return;
                }
                else{
                    //Stalemate
                    MessageBox.Show("Game Over,Stalemate :((.");
                    ReStart();
                    return;
                }
            }

            foreach (Chess Jekll in Shiro.Units)
                if ((Jekll.ID != 60229) && (Jekll.CanMoveTo(Kuro.K.calumn, Kuro.K.row)))
                {
                    //White Checkmate
                    MessageBox.Show("Check mate!! White win!!:D");
                    ReStart();
                    return;
                }
            //============================================ Part of AI End =================================================
        }
        public static bool Valid_Test(Char Cal_Start,int Row_Start,Char Cal_End,int Row_End) {

            int ID_Start = IDMatrix[8 - Row_Start][Cal_Start - 97];
            int ID_End = IDMatrix[8 - Row_End][Cal_End - 97];
            Chess KingChk = (ID_Start / 6) == 1 ? Shiro.K : Kuro.K;

            if (ID_End > 0) { 
                //Function_Picked Mode
                // ============================== Simulation Start ============================== 
                IDMatrix[8 - Row_Start][Cal_Start - 97] = 0;
                IDMatrix[8 - Row_End][Cal_End - 97] = ID_Start;

                if ((ID_Start % 6) == 5)
                {
                    //King
                    if (Be_Attacking(Cal_End, Row_End, KingChk.ClanID_E))
                    {
                        IDMatrix[8 - Row_End][Cal_End - 97] = ID_End;
                        IDMatrix[8 - Row_Start][Cal_Start - 97] = ID_Start;
                        return false;
                    }
                }
                else
                {
                    if (Cal_Start == KingChk.calumn)
                    {
                        //Same-calumn
                        int delta = (Row_Start - KingChk.row > 0 ? 1 : -1);
                        for (int ro = KingChk.row + delta; ; ro += delta)
                        {
                            int ID_Temp = IDat_CI(Cal_Start, ro);
                            if (ID_Temp == 0) continue;
                            else if (ID_Temp == (KingChk.ClanID_E * 6 + 2) || ID_Temp == (KingChk.ClanID_E * 6 + 4))
                            {
                                IDMatrix[8 - Row_End][Cal_End - 97] = ID_End;
                                IDMatrix[8 - Row_Start][Cal_Start - 97] = ID_Start;
                                return false;
                            }
                            break;
                        }
                    }
                    else if (Row_Start == KingChk.row)
                    {
                        //Same-row
                        int delta = (Cal_Start - KingChk.calumn > 0 ? 1 : -1);
                        for (int cal = KingChk.calumn + delta; ; cal += delta)
                        {
                            int ID_Temp = IDat_II(cal - 97, 8 - Row_Start);
                            if (ID_Temp == 0) continue;
                            else if (ID_Temp == (KingChk.ClanID_E * 6 + 2) || ID_Temp == (KingChk.ClanID_E * 6 + 4))
                            {
                                IDMatrix[8 - Row_End][Cal_End - 97] = ID_End;
                                IDMatrix[8 - Row_Start][Cal_Start - 97] = ID_Start;
                                return false;
                            }
                            break;
                        }
                    }
                    else if (Math.Abs(Cal_Start - KingChk.calumn) == Math.Abs(Row_Start - KingChk.row))
                    {
                        int _cal_Differ = (Cal_Start - KingChk.calumn) > 0 ? 1 : -1;
                        int _row_Differ = (Row_Start - KingChk.row) > 0 ? 1 : -1;

                        for (int cal = KingChk.calumn + _cal_Differ, ro = KingChk.row + _row_Differ; ; cal += _cal_Differ, ro += _row_Differ)
                        {
                            int ID_Temp = IDat_II(cal - 97, 8 - ro);
                            if (ID_Temp == 0) continue;
                            else if (ID_Temp == (KingChk.ClanID_E * 6 + 3) || ID_Temp == (KingChk.ClanID_E * 6 + 4))
                            {
                                IDMatrix[8 - Row_End][Cal_End - 97] = ID_End;
                                IDMatrix[8 - Row_Start][Cal_Start - 97] = ID_Start;
                                return false;
                            }
                            break;
                        }
                    }

                    if (Checking)
                        foreach (Chess Checker in CheckingList)
                            if (IDMatrix[8 - Checker.row][Checker.calumn - 97] == Checker.ID)
                                if (Checker.CanMoveTo(KingChk.calumn, KingChk.row)){
                                    IDMatrix[8 - Row_End][Cal_End - 97] = ID_End;
                                    IDMatrix[8 - Row_Start][Cal_Start - 97] = ID_Start;
                                    return false;
                                }
                }
                //Recover Board's Data cuz the step is valid.
                IDMatrix[8 - Row_End][Cal_End - 97] = ID_End;
                IDMatrix[8 - Row_Start][Cal_Start - 97] = ID_Start;
                // =============================== Sinulation End ===============================
            }
            else if (ID_End == 0) { 
                //Function_HintMap_MouseClick Mode
                // ============================== Simulation Start ============================== 
                IDMatrix[8 - Row_End][Cal_End - 97] = ID_Start;
                IDMatrix[8 - Row_Start][Cal_Start - 97] = 0;

                if (ID_Start % 6 == 5){
                    //King
                    if (Math.Abs(Cal_End - Cal_Start) != 2)
                    {
                        //Castling's Check function've included invalid step checking.:D

                        if (Be_Attacking(Cal_End, Row_End, KingChk.ClanID_E))
                        {
                            IDMatrix[8 - Row_End][Cal_End - 97] = 0;
                            IDMatrix[8 - Row_Start][Cal_Start - 97] = ID_Start;
                            return false;
                        }
                    }
                }
                else if ((ID_Start % 6 == 0) && (Cal_End != Cal_Start)){
                    //enpassant
                    IDMatrix[8 - WhoJustStep2.row][WhoJustStep2.calumn - 97] = 0;
                    
                    if (Cal_Start == KingChk.calumn)
                    {
                        //Same-calumn
                        int delta = (Row_Start - KingChk.row > 0 ? 1 : -1);
                        for (int ro = KingChk.row + delta; ; ro += delta)
                        {
                            int ID_Temp = IDat_CI(Cal_Start, ro);
                            if (ID_Temp == 0) continue;
                            else if (ID_Temp == (KingChk.ClanID_E * 6 + 2) || ID_Temp == (KingChk.ClanID_E * 6 + 4))
                            {
                                IDMatrix[8 - WhoJustStep2.row][WhoJustStep2.calumn - 97] = WhoJustStep2.ID;
                                IDMatrix[8 - Row_End][Cal_End - 97] = 0;
                                IDMatrix[8 - Row_Start][Cal_Start - 97] = ID_Start;
                                return false;
                            }
                            break;
                        }
                    }
                    else if (Row_Start == KingChk.row)
                    {
                        //Same-row
                        int delta = (Cal_Start - KingChk.calumn > 0 ? 1 : -1);
                        for (int cal = KingChk.calumn + delta; ; cal += delta)
                        {
                            int ID_Temp = IDat_II(cal - 97, 8 - Row_Start);
                            if (ID_Temp == 0) continue;
                            else if (ID_Temp == (KingChk.ClanID_E * 6 + 2) || ID_Temp == (KingChk.ClanID_E * 6 + 4))
                            {
                                IDMatrix[8 - WhoJustStep2.row][WhoJustStep2.calumn - 97] = WhoJustStep2.ID;
                                IDMatrix[8 - Row_End][Cal_End - 97] = 0;
                                IDMatrix[8 - Row_Start][Cal_Start - 97] = ID_Start;
                                return false;
                            }
                            break;
                        }
                    }
                    else if (Math.Abs(Cal_Start - KingChk.calumn) == Math.Abs(Row_Start - KingChk.row))
                    {
                        int _cal_Differ = (Cal_Start - KingChk.calumn) > 0 ? 1 : -1;
                        int _row_Differ = (Row_Start - KingChk.row) > 0 ? 1 : -1;

                        for (int cal = KingChk.calumn + _cal_Differ, ro = KingChk.row + _row_Differ; ; cal += _cal_Differ, ro += _row_Differ)
                        {
                            int ID_Temp = IDat_II(cal - 97, 8 - ro);
                            if (ID_Temp == 0) continue;
                            else if (ID_Temp == (KingChk.ClanID_E * 6 + 3) || ID_Temp == (KingChk.ClanID_E * 6 + 4))
                            {
                                IDMatrix[8 - WhoJustStep2.row][WhoJustStep2.calumn - 97] = WhoJustStep2.ID;
                                IDMatrix[8 - Row_End][Cal_End - 97] = 0;
                                IDMatrix[8 - Row_Start][Cal_Start - 97] = ID_Start;
                                return false;
                            }
                            break;
                        }
                    }

                    //Extra Check
                    if (Math.Abs(WhoJustStep2.calumn - KingChk.calumn) == Math.Abs(WhoJustStep2.row - KingChk.row))
                    {
                        int _cal_Differ = (WhoJustStep2.calumn - KingChk.calumn) > 0 ? 1 : -1;
                        int _row_Differ = (WhoJustStep2.row - KingChk.row) > 0 ? 1 : -1;

                        for (int cal = KingChk.calumn + _cal_Differ, ro = KingChk.row + _row_Differ; ; cal += _cal_Differ, ro += _row_Differ)
                        {
                            int ID_Temp = IDat_II(cal - 97, 8 - ro);
                            if (ID_Temp == 0) continue;
                            else if (ID_Temp == (KingChk.ClanID_E * 6 + 3) || ID_Temp == (KingChk.ClanID_E * 6 + 4))
                            {
                                IDMatrix[8 - WhoJustStep2.row][WhoJustStep2.calumn - 97] = WhoJustStep2.ID;
                                IDMatrix[8 - Row_End][Cal_End - 97] = 0;
                                IDMatrix[8 - Row_Start][Cal_Start - 97] = ID_Start;
                                return false;
                            }
                            break;
                        }
                    }

                    if (Checking)
                        foreach (Chess Checker in CheckingList)
                            if ((Checker != WhoJustStep2) && Checker.CanMoveTo(KingChk.calumn, KingChk.row))
                            {
                                IDMatrix[8 - WhoJustStep2.row][WhoJustStep2.calumn - 97] = WhoJustStep2.ID;
                                IDMatrix[8 - Row_End][Cal_End - 97] = 0;
                                IDMatrix[8 - Row_Start][Cal_Start - 97] = ID_Start;
                                return false;
                            }

                    IDMatrix[8 - WhoJustStep2.row][WhoJustStep2.calumn - 97] = WhoJustStep2.ID;
                }
                else
                {
                    if (Cal_Start == KingChk.calumn)
                    {
                        //Same-calumn
                        int delta = (Row_Start - KingChk.row > 0 ? 1 : -1);
                        for (int ro = KingChk.row + delta; ; ro += delta)
                        {
                            int ID_Temp = IDat_CI(Cal_Start, ro);
                            if (ID_Temp == 0) continue;
                            else if (ID_Temp == (KingChk.ClanID_E * 6 + 2) || ID_Temp == (KingChk.ClanID_E * 6 + 4))
                            {
                                IDMatrix[8 - Row_End][Cal_End - 97] = 0;
                                IDMatrix[8 - Row_Start][Cal_Start - 97] = ID_Start;
                                return false;
                            }
                            break;
                        }
                    }
                    else if (Row_Start == KingChk.row)
                    {
                        //Same-row
                        int delta = (Cal_Start - KingChk.calumn > 0 ? 1 : -1);
                        for (int cal = KingChk.calumn + delta; ; cal += delta)
                        {
                            int ID_Temp = IDat_II(cal - 97, 8 - Row_Start);
                            if (ID_Temp == 0) continue;
                            else if (ID_Temp == (KingChk.ClanID_E * 6 + 2) || ID_Temp == (KingChk.ClanID_E * 6 + 4))
                            {
                                IDMatrix[8 - Row_End][Cal_End - 97] = 0;
                                IDMatrix[8 - Row_Start][Cal_Start - 97] = ID_Start;
                                return false;
                            }
                            break;
                        }
                    }
                    else if (Math.Abs(Cal_Start - KingChk.calumn) == Math.Abs(Row_Start - KingChk.row))
                    {
                        int _cal_Differ = (Cal_Start - KingChk.calumn) > 0 ? 1 : -1;
                        int _row_Differ = (Row_Start - KingChk.row) > 0 ? 1 : -1;

                        for (int cal = KingChk.calumn + _cal_Differ, ro = KingChk.row + _row_Differ; ; cal += _cal_Differ, ro += _row_Differ)
                        {
                            int ID_Temp = IDat_II(cal - 97, 8 - ro);
                            if (ID_Temp == 0) continue;
                            else if (ID_Temp == (KingChk.ClanID_E * 6 + 3) || ID_Temp == (KingChk.ClanID_E * 6 + 4))
                            {
                                IDMatrix[8 - Row_End][Cal_End - 97] = 0;
                                IDMatrix[8 - Row_Start][Cal_Start - 97] = ID_Start;
                                return false;
                            }
                            break;
                        }
                    }

                    if (Checking)
                        foreach (Chess Checker in CheckingList)
                            if (Checker.CanMoveTo(KingChk.calumn, KingChk.row))
                            {
                                IDMatrix[8 - Row_End][Cal_End - 97] = 0;
                                IDMatrix[8 - Row_Start][Cal_Start - 97] = ID_Start;
                                return false;
                            }
                }
                //Recover Board's Data cuz the step is valid.
                IDMatrix[8 - Row_End][Cal_End - 97] = 0;
                IDMatrix[8 - Row_Start][Cal_Start- 97] = ID_Start;
                // =============================== Sinulation End ===============================
            }
            return true;
        }

        protected static void DrawHint(char _calarg, int _rowarg)
        {
            using (Graphics g = Graphics.FromImage(Form1.hintMapPIC))
            {
                g.FillRectangle(HintBrush, (_calarg - 97) * Chess.chess_Size, (8 - _rowarg) * Chess.chess_Size, Chess.chess_Size, Chess.chess_Size);
            }
        }
        protected static void DrawHint(int _calarg, int _rowarg)
        {
            using (Graphics g = Graphics.FromImage(Form1.hintMapPIC))
            {
                g.FillRectangle(HintBrush, _calarg * Chess.chess_Size, _rowarg * Chess.chess_Size, Chess.chess_Size, Chess.chess_Size);
            }
        }
        public static void DrawTrack(char _calarg, int _rowarg)
        {
            using (Graphics g = Graphics.FromImage(Form1.trackMapPIC))
            {
                g.FillRectangle(TrackBrush, (_calarg - 97) * Chess.chess_Size, (8 - _rowarg) * Chess.chess_Size, Chess.chess_Size, Chess.chess_Size);
            }
        }
        public static void DrawTrack(int _calarg, int _rowarg)
        {
            using (Graphics g = Graphics.FromImage(Form1.trackMapPIC))
            {
                g.FillRectangle(TrackBrush, _calarg * Chess.chess_Size, _rowarg * Chess.chess_Size, Chess.chess_Size, Chess.chess_Size);
            }
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReStart();
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public static void ReStart() {

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    IDMatrix[i][j] = 0;

            Chess_Picked.Color_Default = Color.Transparent;
            Chess_Picked.BackColor = Color.Transparent;

            En_passantable = false;
            Checking = false;
            CheckingList.Clear();
            Picking = false;
            GameOver = true;

            Valhalla_holy.Release();
            Valhalla_inferno.Release();

            Shiro.Stand();
            Kuro.Stand();

            using (Graphics g = Graphics.FromImage(hintMapPIC)) {
                g.Clear(Color.Transparent);
                HintMap.Image = hintMapPIC;
            }
            using (Graphics g = Graphics.FromImage(trackMapPIC))
            {
                g.Clear(Color.Transparent);
                TrackMap.Image = trackMapPIC;
            }

            Recorder.Clear();
            Turn = 1;
        }

        public static void AI_Move(string MoveInfo)
        {
            //Step1.Get the location of start n end.
            char cal_Start = MoveInfo.ElementAt(0), cal_End = MoveInfo.ElementAt(2);
            int row_Start = MoveInfo.ElementAt(1) - 48, row_End = MoveInfo.ElementAt(3) - 48;

            //Step2.Pick who
            foreach (Chess _chess in Kuro.Units)
            {
                if ((_chess.calumn == cal_Start) && (_chess.row == row_Start))
                    if (_chess.ID != 60229) Chess_Picked = _chess;
            }

            //Step3.Eat or step
            if (IDMatrix[8-row_End][cal_End-97]>0)
            {
                //Find who would be eaten.
                foreach (Chess _chess in Shiro.Units)
                {
                    if ((_chess.calumn == cal_End) && (_chess.row == row_End))
                        if (_chess.ID != 60229) Chess_Eaten = _chess;
                }

                using (Graphics g = Graphics.FromImage(Form1.trackMapPIC))
                {
                    g.Clear(Color.Transparent);
                }

                if (Chess_Picked.ID == 12 && row_End == 1) { 
                    //Evolve
                    Chess_Picked.ID = AI_San.EvolveID;
                    switch (Chess_Picked.ID) {
                        case 13: Chess_Picked.Image = Chessing_UI.Properties.Resources.bKn; break;
                        case 14: Chess_Picked.Image = Chessing_UI.Properties.Resources.bR; break;
                        case 15: Chess_Picked.Image = Chessing_UI.Properties.Resources.bB; break;
                        case 16: Chess_Picked.Image = Chessing_UI.Properties.Resources.bQ; break;
                    }
                }
                IDMatrix[8 - row_Start][cal_Start - 97] = 0;
                DrawTrack(cal_Start, row_Start);   //Mark the old location

                Chess_Picked.SetTo(cal_End, row_End);
                DrawTrack(cal_End, row_End);   //Mark the new location

                Chess_Picked.Moved = true;
                En_passantable = false;
                Chess_Eaten.Entomb();
                Form1.TrackMap.Image = Form1.trackMapPIC;
            }
            else
            {
                En_passantable = false;

                if (Chess_Picked.ID == 12)
                {
                    //Check for enpassant or evolve
                    if (row_End - row_Start == -2)
                    {
                        En_passantable = true;
                        WhoJustStep2 = (Pawn)Chess_Picked;
                    }
                    else if (cal_End != cal_Start)
                    {
                        IDMatrix[8 - WhoJustStep2.row][WhoJustStep2.calumn - 97] = 0;
                        WhoJustStep2.Entomb();
                    }
                    else if (row_End == 1) { 
                        //Evolve
                        Chess_Picked.ID = AI_San.EvolveID;
                        switch (Chess_Picked.ID)
                        {
                            case 13: Chess_Picked.Image = Chessing_UI.Properties.Resources.bKn; break;
                            case 14: Chess_Picked.Image = Chessing_UI.Properties.Resources.bR; break;
                            case 15: Chess_Picked.Image = Chessing_UI.Properties.Resources.bB; break;
                            case 16: Chess_Picked.Image = Chessing_UI.Properties.Resources.bQ; break;
                        }
                    }
                }
                else if (Chess_Picked.ID==17 && (Math.Abs(cal_Start-cal_End) == 2))
                {
                    //Find who is the guard
                    if (cal_End == 'c') WhoGuarding = Kuro.R1;
                    else WhoGuarding = Kuro.R2;

                    IDMatrix[8 - WhoGuarding.row][WhoGuarding.calumn - 97] = 0;
                    WhoGuarding.Moved = true;
                    if (cal_End == 'c') WhoGuarding.SetTo('d', 8);
                    else WhoGuarding.SetTo('f', 8);
                }
                //
                using (Graphics g = Graphics.FromImage(Form1.trackMapPIC))
                {
                    g.Clear(Color.Transparent);
                }

                IDMatrix[8 - row_Start][cal_Start - 97] = 0;
                DrawTrack(cal_Start, row_Start);

                Chess_Picked.SetTo(cal_End, row_End);
                DrawTrack(cal_End, row_End);    
                Chess_Picked.Moved = true;
                
                Form1.TrackMap.Image = Form1.trackMapPIC;         
            }
        }
    }
    
}
