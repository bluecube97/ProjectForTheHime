import json
import os

class Daughter:
    def __init__(self):
        self._name = None
        self._age = None
        self._sex = None
        self._mbti = None
        self._hp = None
        self._mp = None
        self._mood = None
        self._stress = None
        self._fatigue = None
        self._E = None
        self._I = None
        self._S = None
        self._N = None
        self._T = None
        self._F = None
        self._J = None
        self._P = None

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


    def load_daughter_status():
        daughter = Daughter()
        ##파일 경로 설정
        communication_path = os.path.join("conversationData", "daughter_status.json")
        with open(communication_path, 'r', encoding='utf-8') as f:
            items = json.load(f)

        status_keys = items["daughter"]

        # 예외처리 key값이 없을경우
        daughter.name = status_keys["name"]
        daughter.age = status_keys["age"]
        daughter.sex = status_keys["sex"]
        daughter.mbti = status_keys["mbti"]
        daughter.hp = status_keys["hp"]
        daughter.mp = status_keys["mp"]
        daughter.mood = status_keys["mood"]
        daughter.stress = status_keys["stress"]
        daughter.fatigue = status_keys["fatigue"]
        daughter.E = status_keys["E"]
        daughter.I = status_keys["I"]
        daughter.S = status_keys["S"]
        daughter.N = status_keys["N"]
        daughter.T = status_keys["T"]
        daughter.F = status_keys["F"]
        daughter.J = status_keys["J"]
        daughter.P = status_keys["P"]

        #print("딸랑구 나이 : ", daughter.age)
        #print("딸랑구 이름 : ", daughter.name)  # 이제 업데이트된 age가 출력됩니다

        return daughter