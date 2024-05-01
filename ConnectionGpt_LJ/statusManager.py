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
    def get_Name(self):
        return self._name

    @get_Name.setter
    def set_Name(self, value):
        self._name = value

    @property
    def get_Age(self):
        return self._age

    @get_Age.setter
    def set_Age(self, value):
        self._age = value

    @property
    def get_Sex(self):
        return self._sex

    @get_Sex.setter
    def set_Sex(self, value):
        self._sex = value

    @property
    def get_Mbti(self):
        return self._mbti

    @get_Mbti.setter
    def set_Mbti(self, value):
        self._mbti = value

    @property
    def get_Hp(self):
        return self._hp

    @get_Hp.setter
    def set_Hp(self, value):
        self._hp = value

    @property
    def get_Mp(self):
        return self._mp

    @get_Mp.setter
    def set_Mp(self, value):
        self._mp = value

    @property
    def get_Mood(self):
        return self._mood

    @get_Mood.setter
    def set_Mood(self, value):
        self._mood = value

    @property
    def get_Stress(self):
        return self._stress

    @get_Stress.setter
    def set_Stress(self, value):
        self._stress = value

    @property
    def get_Fatigue(self):
        return self._fatigue

    @get_Fatigue.setter
    def set_Fatigue(self, value):
        self._fatigue = value

    @property
    def get_E(self):
        return self._E

    @get_E.setter
    def set_E(self, value):
        self._E = value

    @property
    def get_I(self):
        return self._I

    @get_I.setter
    def set_I(self, value):
        self._I = value

    @property
    def get_S(self):
        return self._S

    @get_S.setter
    def set_S(self, value):
        self._S = value

    @property
    def get_N(self):
        return self._N

    @get_N.setter
    def set_N(self, value):
        self._N = value

    @property
    def get_T(self):
        return self._T

    @get_T.setter
    def set_T(self, value):
        self._T = value

    @property
    def get_F(self):
        return self._F

    @get_F.setter
    def set_F(self, value):
        self._F = value

    @property
    def get_J(self):
        return self._J

    @get_J.setter
    def set_J(self, value):
        self._J = value

    @property
    def get_P(self):
        return self._P

    @get_P.setter
    def set_P(self, value):
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

def load_daughter_status(daughter, communication_path):
    daughter = Daughter()
    with open(communication_path, 'r', encoding='utf-8') as f:
        items = json.load(f)

    status_keys = items["daughter"]

    # 예외처리 key값이 없을경우
    daughter.set_Name = status_keys["name"]
    daughter.set_Age = status_keys["age"]
    daughter.set_Sex = status_keys["sex"]
    daughter.set_Mbti = status_keys["mbti"]
    daughter.set_Hp = status_keys["hp"]
    daughter.set_Mp = status_keys["mp"]
    daughter.set_Mood = status_keys["mood"]
    daughter.set_Stress = status_keys["stress"]
    daughter.set_Fatigue = status_keys["fatigue"]
    daughter.set_E = status_keys["E"]
    daughter.set_I = status_keys["I"]
    daughter.set_S = status_keys["S"]
    daughter.set_N = status_keys["N"]
    daughter.set_T = status_keys["T"]
    daughter.set_F = status_keys["F"]
    daughter.set_J = status_keys["J"]
    daughter.set_P = status_keys["P"]

    return daughter

d = Daughter()

##파일 경로 설정
communication_path = os.path.join("conversationData", "daughter_status.json")
d = load_daughter_status(d, communication_path)

print("딸랑구 나이 : ", d.get_Age)
print("딸랑구 이름 : ", d.get_Name)  # 이제 업데이트된 age가 출력됩니다
