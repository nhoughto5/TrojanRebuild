using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrojanWebRebuild.Models;

namespace TrojanWebRebuild.Logic
{
    public class Severity
    {
        List<int> attributes = new List<int>();
        List<int> insert = new List<int>();
        List<int> abstraction = new List<int>();
        List<int> effect = new List<int>();
        List<int> logictype = new List<int>();
        List<int> functionality = new List<int>();
        List<int> activation = new List<int>();
        List<int> physicalLayout = new List<int>();
        List<int> location = new List<int>();

        public Severity(List<int> attr)
        {
            attributes = attr;
            organize();
        }
        public SeverityRating getSevRating()
        {
            SeverityRating Severity = new SeverityRating();
            Severity.iR = iR();
            Severity.iA = iA();
            Severity.iE = iE();
            Severity.iL = iL();
            Severity.iF = iF();
            Severity.iC = iC();
            Severity.iP = iP();
            Severity.iO = iO();

            Severity.cR = cR();
            Severity.cA = cA();
            Severity.cE = cE();
            Severity.cL = cL();
            Severity.cF = cF();
            Severity.cC = cC();
            Severity.cP = cP();
            Severity.cO = cO();
            return Severity;
        }
        private void organize()
        {
            insert = attributes.Where(s => s <= 5).ToList();
            abstraction = attributes.Where(s => (s > 5) && (s < 12)).ToList();
            effect = attributes.Where(s => (s > 11) && (s < 16)).ToList();
            logictype = attributes.Where(s => (s > 15) && (s < 18)).ToList();
            functionality = attributes.Where(s => (s > 17) && (s < 20)).ToList();
            activation = attributes.Where(s => (s > 19) && (s < 23)).ToList();
            physicalLayout = attributes.Where(s => (s > 22) && (s < 29)).ToList();
            location = attributes.Where(s => (s > 28)).ToList();
        }
        public string iR()
        {
            int min = insert.Min();
            switch (min)
            {
                case 1:
                    return "5";
                case 2:
                    return "4";
                case 3:
                    return "3";
                case 4:
                    return "2";
                case 5:
                    return "1";
                default:
                    return "NA";
            }
        }
        public string iA()
        {
            int min = abstraction.Min();
            switch (min)
            {
                case 6:
                    return "6";
                case 7:
                    return "5";
                case 8:
                    return "4";
                case 9:
                    return "3";
                case 10:
                    return "2";
                case 11:
                    return "1";
                default:
                    return "NA";
            }
        }

        public string cR()
        {
            return iR();
        }
        public string cA()
        {
            return iA();
        }

        public string iE()
        {
            //12
            if (effect.Contains(12) && !effect.Contains(13) && !effect.Contains(14) && !effect.Contains(15))
            {
                return "1";
            }
            //13
            else if (!effect.Contains(12) && effect.Contains(13) && !effect.Contains(14) && !effect.Contains(15))
            {
                return "2";
            }
            //14
            else if (!effect.Contains(12) && !effect.Contains(13) && effect.Contains(14) && !effect.Contains(15))
            {
                return "3";
            }
            //15
            else if (!effect.Contains(12) && !effect.Contains(13) && !effect.Contains(14) && effect.Contains(15))
            {
                return "4";
            }
            //12 & 13
            else if (effect.Contains(12) && effect.Contains(13) && !effect.Contains(14) && !effect.Contains(15))
            {
                return "5";
            }
            //12 & 14
            else if (effect.Contains(12) && !effect.Contains(13) && effect.Contains(14) && !effect.Contains(15))
            {
                return "6";
            }
            //12 & 15
            else if (effect.Contains(12) && !effect.Contains(13) && !effect.Contains(14) && effect.Contains(15))
            {
                return "7";
            }
            //13 & 14 
            else if (!effect.Contains(12) && effect.Contains(13) && effect.Contains(14) && !effect.Contains(15))
            {
                return "8";
            }
            //13 & 15
            else if (!effect.Contains(12) && effect.Contains(13) && !effect.Contains(14) && effect.Contains(15))
            {
                return "9";
            }
            //14 & 15
            else if (!effect.Contains(12) && !effect.Contains(13) && effect.Contains(14) && effect.Contains(15))
            {
                return "A";
            }
            //12, 13, 14
            else if (effect.Contains(12) && effect.Contains(13) && effect.Contains(14) && !effect.Contains(15))
            {
                return "B";
            }
            //12, 13, 15
            else if (effect.Contains(12) && effect.Contains(13) && !effect.Contains(14) && effect.Contains(15))
            {
                return "C";
            }
            //12, 14, 15
            else if (effect.Contains(12) && !effect.Contains(13) && effect.Contains(14) && effect.Contains(15))
            {
                return "D";
            }
            //13, 14, 15
            else if (!effect.Contains(12) && effect.Contains(13) && effect.Contains(14) && effect.Contains(15))
            {
                return "E";
            }
            //12, 13, 14, 15
            else if (effect.Contains(12) && effect.Contains(13) && effect.Contains(14) && effect.Contains(15))
            {
                return "F";
            }
            else return "NA";

        }
        public string cE()
        {
            //12
            if (effect.Contains(12) && !effect.Contains(13) && !effect.Contains(14) && !effect.Contains(15))
            {
                return "2";
            }
            //13
            else if (!effect.Contains(12) && effect.Contains(13) && !effect.Contains(14) && !effect.Contains(15))
            {
                return "4";
            }
            //14
            else if (!effect.Contains(12) && !effect.Contains(13) && effect.Contains(14) && !effect.Contains(15))
            {
                return "1";
            }
            //15
            else if (!effect.Contains(12) && !effect.Contains(13) && !effect.Contains(14) && effect.Contains(15))
            {
                return "2";
            }
            //12 & 13
            else if (effect.Contains(12) && effect.Contains(13) && !effect.Contains(14) && !effect.Contains(15))
            {
                return "6";
            }
            //12 & 14
            else if (effect.Contains(12) && !effect.Contains(13) && effect.Contains(14) && !effect.Contains(15))
            {
                return "3";
            }
            //12 & 15
            else if (effect.Contains(12) && !effect.Contains(13) && !effect.Contains(14) && effect.Contains(15))
            {
                return "4";
            }
            //13 & 14 
            else if (!effect.Contains(12) && effect.Contains(13) && effect.Contains(14) && !effect.Contains(15))
            {
                return "5";
            }
            //13 & 15
            else if (!effect.Contains(12) && effect.Contains(13) && !effect.Contains(14) && effect.Contains(15))
            {
                return "6";
            }
            //14 & 15
            else if (!effect.Contains(12) && !effect.Contains(13) && effect.Contains(14) && effect.Contains(15))
            {
                return "3";
            }
            //12, 13, 14
            else if (effect.Contains(12) && effect.Contains(13) && effect.Contains(14) && !effect.Contains(15))
            {
                return "7";
            }
            //12, 13, 15
            else if (effect.Contains(12) && effect.Contains(13) && !effect.Contains(14) && effect.Contains(15))
            {
                return "8";
            }
            //12, 14, 15
            else if (effect.Contains(12) && !effect.Contains(13) && effect.Contains(14) && effect.Contains(15))
            {
                return "5";
            }
            //13, 14, 15
            else if (!effect.Contains(12) && effect.Contains(13) && effect.Contains(14) && effect.Contains(15))
            {
                return "7";
            }
            //12, 13, 14, 15
            else if (effect.Contains(12) && effect.Contains(13) && effect.Contains(14) && effect.Contains(15))
            {
                return "9";
            }
            else return "NA";

        }

        public string iL()
        {
            //16
            if (logictype.Contains(16) && !logictype.Contains(17))
            {
                return "2";
            }
            //17
            else if (!logictype.Contains(16) && logictype.Contains(17))
            {
                return "1";
            }
            //16 & 17
            else if (logictype.Contains(16) && logictype.Contains(17))
            {
                return "3";
            }
            else return "NA";
        }
        public string cL()
        {
            return iL();
        }

        public string iF()
        {
            //18
            if (functionality.Contains(18) && !functionality.Contains(19))
            {
                return "1";
            }
            //19
            else if (!functionality.Contains(18) && functionality.Contains(19))
            {
                return "2";
            }
            //18 & 19
            else if (functionality.Contains(18) && functionality.Contains(19))
            {
                return "3";
            }
            else return "NA";
        }
        public string cF()
        {
            return iF();
        }

        public string iC()
        {
            if (activation.Contains(20) && !activation.Contains(21) && !activation.Contains(22))
            {
                return "1";
            }
            else if (!activation.Contains(20) && activation.Contains(21) && !activation.Contains(22))
            {
                return "2";
            }
            else if (!activation.Contains(20) && !activation.Contains(21) && activation.Contains(22))
            {
                return "3";
            }
            else if (activation.Contains(20) && activation.Contains(21) && !activation.Contains(22))
            {
                return "4";
            }
            else if (activation.Contains(20) && !activation.Contains(21) && activation.Contains(22))
            {
                return "5";
            }
            else if (!activation.Contains(20) && activation.Contains(21) && activation.Contains(22))
            {
                return "6";
            }
            else if (activation.Contains(20) && activation.Contains(21) && activation.Contains(22))
            {
                return "7";
            }
            else return "NA";
        }
        public string cC()
        {
            if (activation.Contains(20) && !activation.Contains(21) && !activation.Contains(22))
            {
                return "1";
            }
            else if (!activation.Contains(20) && activation.Contains(21) && !activation.Contains(22))
            {
                return "2";
            }
            else if (!activation.Contains(20) && !activation.Contains(21) && activation.Contains(22))
            {
                return "4";
            }
            else if (activation.Contains(20) && activation.Contains(21) && !activation.Contains(22))
            {
                return "3";
            }
            else if (activation.Contains(20) && !activation.Contains(21) && activation.Contains(22))
            {
                return "5";
            }
            else if (!activation.Contains(20) && activation.Contains(21) && activation.Contains(22))
            {
                return "6";
            }
            else if (activation.Contains(20) && activation.Contains(21) && activation.Contains(22))
            {
                return "7";
            }
            else return "NA";
        }

        public string iP()
        {
            Dictionary<string, int> group = pGroup(physicalLayout.Contains(23), physicalLayout.Contains(24), physicalLayout.Contains(25), physicalLayout.Contains(26), physicalLayout.Contains(27), physicalLayout.Contains(28));
            string answer = null;
            //1.) 0,0,0
            if (group["size"] == 0 && group["layout"] == 0 && group["Dist"] == 0)
            {
                answer = "NA";
            }
            //2.) 0,0,1
            else if (group["size"] == 0 && group["layout"] == 0 && group["Dist"] == 1)
            {
                answer = "-";
            }
            //3.) 0,0,2
            else if (group["size"] == 0 && group["layout"] == 0 && group["Dist"] == 2)
            {
                answer = "-";
            }
            //4.) 0,1,0
            else if (group["size"] == 0 && group["layout"] == 1 && group["Dist"] == 0)
            {
                answer = "-";
            }
            //5.) 0,1,1
            else if (group["size"] == 0 && group["layout"] == 1 && group["Dist"] == 1)
            {
                answer = "1";
            }
            //6.) 0,1,2
            else if (group["size"] == 0 && group["layout"] == 1 && group["Dist"] == 2)
            {
                answer = "2";
            }
            //7.) 0,2,0
            else if (group["size"] == 0 && group["layout"] == 2 && group["Dist"] == 0)
            {
                answer = "-";
            }
            //8.) 0,2,1
            else if (group["size"] == 0 && group["layout"] == 2 && group["Dist"] == 1)
            {
                answer = "3";
            }
            //9.) 0,2,2
            else if (group["size"] == 0 && group["layout"] == 2 && group["Dist"] == 2)
            {
                answer = "4";
            }
            //10.) 1,0,0
            else if (group["size"] == 1 && group["layout"] == 0 && group["Dist"] == 0)
            {
                answer = "-";
            }
            //11.) 1,0,1
            else if (group["size"] == 1 && group["layout"] == 0 && group["Dist"] == 1)
            {
                answer = "1";
            }
            //12.) 1,0,2
            else if (group["size"] == 1 && group["layout"] == 0 && group["Dist"] == 2)
            {
                answer = "2";
            }
            //13.) 1,1,0
            else if (group["size"] == 1 && group["layout"] == 1 && group["Dist"] == 0)
            {
                answer = "1";
            }
            //14.) 1,1,1
            else if (group["size"] == 1 && group["layout"] == 1 && group["Dist"] == 1)
            {
                answer = "1";
            }
            //15.) 1,1,2
            else if (group["size"] == 1 && group["layout"] == 1 && group["Dist"] == 2)
            {
                answer = "2";
            }
            //16.) 1,2,0
            else if (group["size"] == 1 && group["layout"] == 2 && group["Dist"] == 0)
            {
                answer = "3";
            }
            //17.) 1,2,1
            else if (group["size"] == 1 && group["layout"] == 2 && group["Dist"] == 1)
            {
                answer = "3";
            }
            //18.) 1,2,2
            else if (group["size"] == 1 && group["layout"] == 2 && group["Dist"] == 2)
            {
                answer = "4";
            }
            //19.) 2,0,0
            else if (group["size"] == 2 && group["layout"] == 0 && group["Dist"] == 0)
            {
                answer = "-";
            }
            //20.) 2,0,1
            else if (group["size"] == 2 && group["layout"] == 0 && group["Dist"] == 1)
            {
                answer = "5";
            }
            //21.) 2,0,2
            else if (group["size"] == 2 && group["layout"] == 0 && group["Dist"] == 2)
            {
                answer = "6";
            }
            //22.) 2,1,0
            else if (group["size"] == 2 && group["layout"] == 1 && group["Dist"] == 0)
            {
                answer = "5";
            }
            //23.) 2,1,1
            else if (group["size"] == 2 && group["layout"] == 1 && group["Dist"] == 1)
            {
                answer = "5";
            }
            //24.) 2,1,2
            else if (group["size"] == 2 && group["layout"] == 1 && group["Dist"] == 2)
            {
                answer = "6";
            }
            //25.) 2,2,0
            else if (group["size"] == 2 && group["layout"] == 2 && group["Dist"] == 0)
            {
                answer = "7";
            }
            //26.) 2,2,1
            else if (group["size"] == 2 && group["layout"] == 2 && group["Dist"] == 1)
            {
                answer = "7";
            }
            //27.) 2,2,2
            else if (group["size"] == 2 && group["layout"] == 2 && group["Dist"] == 2)
            {
                answer = "8";
            }
            else answer = "NA";
            return answer;
        }
        public string cP()
        {
            Dictionary<string, int> group = pGroup(physicalLayout.Contains(23), physicalLayout.Contains(24), physicalLayout.Contains(25), physicalLayout.Contains(26), physicalLayout.Contains(27), physicalLayout.Contains(28));
            string answer = null;
            //1.) 0,0,0
            if (group["size"] == 0 && group["layout"] == 0 && group["Dist"] == 0)
            {
                answer = "NA";
            }
            //2.) 0,0,1
            else if (group["size"] == 0 && group["layout"] == 0 && group["Dist"] == 1)
            {
                answer = "1";
            }
            //3.) 0,0,2
            else if (group["size"] == 0 && group["layout"] == 0 && group["Dist"] == 2)
            {
                answer = "2";
            }
            //4.) 0,1,0
            else if (group["size"] == 0 && group["layout"] == 1 && group["Dist"] == 0)
            {
                answer = "1";
            }
            //5.) 0,1,1
            else if (group["size"] == 0 && group["layout"] == 1 && group["Dist"] == 1)
            {
                answer = "3";
            }
            //6.) 0,1,2
            else if (group["size"] == 0 && group["layout"] == 1 && group["Dist"] == 2)
            {
                answer = "4";
            }
            //7.) 0,2,0
            else if (group["size"] == 0 && group["layout"] == 2 && group["Dist"] == 0)
            {
                answer = "2";
            }
            //8.) 0,2,1
            else if (group["size"] == 0 && group["layout"] == 2 && group["Dist"] == 1)
            {
                answer = "4";
            }
            //9.) 0,2,2
            else if (group["size"] == 0 && group["layout"] == 2 && group["Dist"] == 2)
            {
                answer = "5";
            }
            //10.) 1,0,0
            else if (group["size"] == 1 && group["layout"] == 0 && group["Dist"] == 0)
            {
                answer = "1";
            }
            //11.) 1,0,1
            else if (group["size"] == 1 && group["layout"] == 0 && group["Dist"] == 1)
            {
                answer = "3";
            }
            //12.) 1,0,2
            else if (group["size"] == 1 && group["layout"] == 0 && group["Dist"] == 2)
            {
                answer = "4";
            }
            //13.) 1,1,0
            else if (group["size"] == 1 && group["layout"] == 1 && group["Dist"] == 0)
            {
                answer = "3";
            }
            //14.) 1,1,1
            else if (group["size"] == 1 && group["layout"] == 1 && group["Dist"] == 1)
            {
                answer = "3";
            }
            //15.) 1,1,2
            else if (group["size"] == 1 && group["layout"] == 1 && group["Dist"] == 2)
            {
                answer = "4";
            }
            //16.) 1,2,0
            else if (group["size"] == 1 && group["layout"] == 2 && group["Dist"] == 0)
            {
                answer = "4";
            }
            //17.) 1,2,1
            else if (group["size"] == 1 && group["layout"] == 2 && group["Dist"] == 1)
            {
                answer = "4";
            }
            //18.) 1,2,2
            else if (group["size"] == 1 && group["layout"] == 2 && group["Dist"] == 2)
            {
                answer = "5";
            }
            //19.) 2,0,0
            else if (group["size"] == 2 && group["layout"] == 0 && group["Dist"] == 0)
            {
                answer = "2";
            }
            //20.) 2,0,1
            else if (group["size"] == 2 && group["layout"] == 0 && group["Dist"] == 1)
            {
                answer = "4";
            }
            //21.) 2,0,2
            else if (group["size"] == 2 && group["layout"] == 0 && group["Dist"] == 2)
            {
                answer = "5";
            }
            //22.) 2,1,0
            else if (group["size"] == 2 && group["layout"] == 1 && group["Dist"] == 0)
            {
                answer = "4";
            }
            //23.) 2,1,1
            else if (group["size"] == 2 && group["layout"] == 1 && group["Dist"] == 1)
            {
                answer = "4";
            }
            //24.) 2,1,2
            else if (group["size"] == 2 && group["layout"] == 1 && group["Dist"] == 2)
            {
                answer = "5";
            }
            //25.) 2,2,0
            else if (group["size"] == 2 && group["layout"] == 2 && group["Dist"] == 0)
            {
                answer = "5";
            }
            //26.) 2,2,1
            else if (group["size"] == 2 && group["layout"] == 2 && group["Dist"] == 1)
            {
                answer = "5";
            }
            //27.) 2,2,2
            else if (group["size"] == 2 && group["layout"] == 2 && group["Dist"] == 2)
            {
                answer = "6";
            }
            else answer = "NA";
            return answer;
        }

        public string iO()
        {
            //29
            if (location.Contains(29) && !location.Contains(30) && !location.Contains(31) && !location.Contains(32) && !location.Contains(33))
            {
                return "1";
            }
            //30
            else if (!location.Contains(29) && location.Contains(30) && !location.Contains(31) && !location.Contains(32) && !location.Contains(33))
            {
                return "2";
            }
            //31
            else if (!location.Contains(29) && !location.Contains(30) && location.Contains(31) && !location.Contains(32) && !location.Contains(33))
            {
                return "3";
            }
            //32
            else if (!location.Contains(29) && !location.Contains(30) && !location.Contains(31) && location.Contains(32) && !location.Contains(33))
            {
                return "4";
            }
            //33
            else if (!location.Contains(29) && !location.Contains(30) && !location.Contains(31) && !location.Contains(32) && location.Contains(33))
            {
                return "5";
            }
            //29, 30
            else if (location.Contains(29) && location.Contains(30) && !location.Contains(31) && !location.Contains(32) && !location.Contains(33))
            {
                return "6";
            }
            //29, 31
            else if (location.Contains(29) && !location.Contains(30) && location.Contains(31) && !location.Contains(32) && !location.Contains(33))
            {
                return "7";
            }
            //29, 32
            else if (location.Contains(29) && !location.Contains(30) && !location.Contains(31) && location.Contains(32) && !location.Contains(33))
            {
                return "8";
            }
            //29, 33
            else if (location.Contains(29) && !location.Contains(30) && !location.Contains(31) && !location.Contains(32) && location.Contains(33))
            {
                return "9";
            }
            //30, 31
            else if (!location.Contains(29) && location.Contains(30) && location.Contains(31) && !location.Contains(32) && !location.Contains(33))
            {
                return "A";
            }
            //30, 32
            else if (!location.Contains(29) && location.Contains(30) && !location.Contains(31) && location.Contains(32) && !location.Contains(33))
            {
                return "B";
            }
            //30, 33
            else if (!location.Contains(29) && location.Contains(30) && !location.Contains(31) && !location.Contains(32) && location.Contains(33))
            {
                return "C";
            }
            //31, 32
            else if (!location.Contains(29) && !location.Contains(30) && location.Contains(31) && location.Contains(32) && !location.Contains(33))
            {
                return "D";
            }
            //31, 33
            else if (!location.Contains(29) && !location.Contains(30) && location.Contains(31) && !location.Contains(32) && location.Contains(33))
            {
                return "E";
            }
            //32, 33
            else if (!location.Contains(29) && !location.Contains(30) && !location.Contains(31) && location.Contains(32) && location.Contains(33))
            {
                return "F";
            }
            //29, 30, 31
            else if (location.Contains(29) && location.Contains(30) && location.Contains(31) && !location.Contains(32) && !location.Contains(33))
            {
                return "G";
            }
            //29, 30, 32
            else if (location.Contains(29) && location.Contains(30) && !location.Contains(31) && location.Contains(32) && !location.Contains(33))
            {
                return "H";
            }
            //29, 30, 33
            else if (location.Contains(29) && location.Contains(30) && !location.Contains(31) && !location.Contains(32) && location.Contains(33))
            {
                return "I";
            }
            //29, 31, 32
            else if (location.Contains(29) && !location.Contains(30) && location.Contains(31) && location.Contains(32) && !location.Contains(33))
            {
                return "J";
            }
            //29, 31, 33
            else if (location.Contains(29) && !location.Contains(30) && location.Contains(31) && !location.Contains(32) && location.Contains(33))
            {
                return "K";
            }
            //29, 32, 33
            else if (location.Contains(29) && !location.Contains(30) && !location.Contains(31) && location.Contains(32) && location.Contains(33))
            {
                return "L";
            }
            //30, 31, 32
            else if (!location.Contains(29) && location.Contains(30) && location.Contains(31) && location.Contains(32) && !location.Contains(33))
            {
                return "M";
            }
            //30, 31, 33
            else if (!location.Contains(29) && location.Contains(30) && location.Contains(31) && !location.Contains(32) && location.Contains(33))
            {
                return "N";
            }
            //30, 32, 33
            else if (!location.Contains(29) && location.Contains(30) && !location.Contains(31) && location.Contains(32) && location.Contains(33))
            {
                return "O";
            }
            //31, 32, 33
            else if (!location.Contains(29) && !location.Contains(30) && location.Contains(31) && location.Contains(32) && location.Contains(33))
            {
                return "P";
            }
            //29, 30, 31, 32
            else if (location.Contains(29) && location.Contains(30) && location.Contains(31) && location.Contains(32) && !location.Contains(33))
            {
                return "Q";
            }
            //29, 30, 31, 33
            else if (location.Contains(29) && location.Contains(30) && location.Contains(31) && !location.Contains(32) && location.Contains(33))
            {
                return "R";
            }
            //29, 30, 32, 33
            else if (location.Contains(29) && location.Contains(30) && !location.Contains(31) && location.Contains(32) && location.Contains(33))
            {
                return "S";
            }
            //29, 31, 32, 33
            else if (location.Contains(29) && !location.Contains(30) && location.Contains(31) && location.Contains(32) && location.Contains(33))
            {
                return "T";
            }
            //30, 31, 32, 33
            else if (!location.Contains(29) && location.Contains(30) && location.Contains(31) && location.Contains(32) && location.Contains(33))
            {
                return "U";
            }
            //29, 30, 31, 32, 33
            else if (location.Contains(29) && location.Contains(30) && location.Contains(31) && location.Contains(32) && location.Contains(33))
            {
                return "V";
            }
            else return "NA";
        }
        public string cO()
        {
            if (location.Contains(29) || location.Contains(30) || location.Contains(31) || location.Contains(32) || location.Contains(33))
            {
                return location.Count().ToString();
            }
            else return "NA";
        }
        public Dictionary<string, int> pGroup(bool twentythree, bool twentyfour, bool twentyfive, bool twentysix, bool twentyseven, bool twentyeight)
        {
            Dictionary<string, int> group = new Dictionary<string, int>();
            //====== Size =====//
            if (twentyfour)
            {
                group.Add("size", 2);
            }
            else if (twentythree && !twentyfour)
            {
                group.Add("size", 1);
            }
            else
            {
                group.Add("size", 0);
            }

            //====== Layout =====//
            if (twentysix)
            {
                group.Add("layout", 2);
            }
            else if (twentyfive && !twentysix)
            {
                group.Add("layout", 1);
            }
            else
            {
                group.Add("layout", 0);
            }

            //====== Distribution =====//
            if (twentyeight)
            {
                group.Add("Dist", 2);
            }
            else if (twentyseven && !twentyeight)
            {
                group.Add("Dist", 1);
            }
            else
            {
                group.Add("Dist", 0);
            }
            return group;
        }
    }
}