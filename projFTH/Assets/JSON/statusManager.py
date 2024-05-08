import json
import os

class Daughter:
    def __init__(self, name=None, age=None, sex=None, mbti=None, hp=None, mp=None, mood=None, stress=None, fatigue=None, E=None, I=None, S=None, N=None, T=None, F=None, J=None, P=None):
        self._name = name
        self._age = age
        self._sex = sex
        self._mbti = mbti
        self._hp = hp
        self._mp = mp
        self._mood = mood
        self._stress = stress
        self._fatigue = fatigue
        self._E = E
        self._I = I
        self._S = S
        self._N = N
        self._T = T
        self._F = F
        self._J = J
        self._P = P

    @property
    def name(self):
        return self._name

    @name.setter
    def name(self, value):
        self._name = value

    @property
    def age(self):
        return self._age

    @age.setter
    def age(self, value):
        self._age = value

    @property
    def sex(self):
        return self._sex

    @sex.setter
    def sex(self, value):
        self._sex = value

    @property
    def mbti(self):
        return self._mbti

    @mbti.setter
    def mbti(self, value):
        self._mbti = value

    @property
    def hp(self):
        return self._hp

    @hp.setter
    def hp(self, value):
        self._hp = value

    @property
    def mp(self):
        return self._mp

    @mp.setter
    def mp(self, value):
        self._mp = value

    @property
    def mood(self):
        return self._mood

    @mood.setter
    def mood(self, value):
        self._mood = value

    @property
    def stress(self):
        return self._stress

    @stress.setter
    def stress(self, value):
        self._stress = value

    @property
    def fatigue(self):
        return self._fatigue

    @fatigue.setter
    def fatigue(self, value):
        self._fatigue = value

    @property
    def E(self):
        return self._E

    @E.setter
    def E(self, value):
        self._E = value

    @property
    def I(self):
        return self._I

    @I.setter
    def I(self, value):
        self._I = value

    @property
    def S(self):
        return self._S

    @S.setter
    def S(self, value):
        self._S = value

    @property
    def N(self):
        return self._N

    @N.setter
    def N(self, value):
        self._N = value

    @property
    def T(self):
        return self._T

    @T.setter
    def T(self, value):
        self._T = value

    @property
    def F(self):
        return self._F

    @F.setter
    def F(self, value):
        self._F = value

    @property
    def J(self):
        return self._J

    @J.setter
    def J(self, value):
        self._J = value

    @property
    def P(self):
        return self._P

    @P.setter
    def P(self, value):
        self._P = value


    def display_stats(self):
        print("Daughter's Stats:")
        print("Name:", self.name)
        print("Age:", self.age)
        print("Sex:", self.sex)
        print("MBTI:", self.mbti)
        print("Health Points (HP):", self.hp)
        print("Magic Points (MP):", self.mp)
        print("Mood:", self.mood)
        print("Stress Level:", self.stress)
        print("Fatigue Level:", self.fatigue)
        print("Extroversion (E):", self.E)
        print("Introversion (I):", self.I)
        print("Sensing (S):", self.S)
        print("Intuition (N):", self.N)
        print("Thinking (T):", self.T)
        print("Feeling (F):", self.F)
        print("Judging (J):", self.J)
        print("Perceiving (P):", self.P)

#d = Daughter() 

def load_daughter_status():
<<<<<<< HEAD
    basedir = os.path.dirname(os.path.abspath(__file__)) #지금 실행되고 있는 파일 위치가 절대경로가 됨
    communication_path = os.path.join("conversationData", "daughter_status.json")
=======
    base_dir = os.path.dirname(os.path.abspath(__file__))
    communication_path = os.path.join(base_dir, "conversationData", "daughter_status.json")
>>>>>>> 02cb1e43966fa80b6aa5cf5ca3759058891f50d0
    try:
        with open(communication_path, 'r', encoding='utf-8') as f:
            items = json.load(f)
            status_keys = items["daughter"]
            return Daughter(
                name=status_keys["name"],
                age=status_keys["age"],
                sex=status_keys["sex"],
                mbti=status_keys["mbti"],
                hp=status_keys["hp"],
                mp=status_keys["mp"],
                mood=status_keys["mood"],
                stress=status_keys["stress"],
                fatigue=status_keys["fatigue"],
                E=status_keys["E"],
                I=status_keys["I"],
                S=status_keys["S"],
                N=status_keys["N"],
                T=status_keys["T"],
                F=status_keys["F"],
                J=status_keys["J"],
                P=status_keys["P"]
            )
    except Exception as e:
        print(f"Error loading daughter status: {e}")
        return Daughter()  # 오류 발생 시 빈 객체 반환


d = load_daughter_status()