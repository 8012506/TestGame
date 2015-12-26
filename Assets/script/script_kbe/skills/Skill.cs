namespace KBEngine
{
  	using UnityEngine; 
	using System; 
	using System.Collections; 
	using System.Collections.Generic;

    public class SkillStub
    {
        public UnityEngine.GameObject renderObj = null;
        public bool isCasting = false;
        public float skillDistance = 0f;
        public float restCastTimer = 0f;
    }

    public class Skill 
    {
    	public string name;
    	public string descr;
    	public Int32 id;
    	public float canUseDistMin = 0f;
    	public float canUseDistMax = 3f;
        public float coolTime = 0.5f;
        //技能释放相关
        private List<SkillStub> skillStubs = new List<SkillStub>();
        public float castTime = 0.5f;
        
        private float restCoolTimer;
		public Skill()
		{
		}
		
		public bool validCast(KBEngine.Entity caster, SCObject target)
        {
			float dist = Vector3.Distance(target.getPosition(), caster.position);
            if (dist > canUseDistMax || restCoolTimer < coolTime || ((SByte)(caster.getDefinedPropterty("state"))) == 1)
				return false;
			
			return true;
		}

        public void updateTimer(float second)
        {
            restCoolTimer += second;
            for(int i = skillStubs.Count-1; i>=0; i--)
            {
                skillStubs[i].restCastTimer += second;
                if (skillStubs[i].renderObj != null && skillStubs[i].isCasting && skillStubs[i].restCastTimer < castTime)
                {
                    skillStubs[i].renderObj.transform.Translate(Vector3.forward * skillStubs[i].skillDistance * second / castTime);
                }
                else
                {
                    skillStubs[i].isCasting = false;
                    if (skillStubs[i].renderObj != null)
                    {
                        UnityEngine.GameObject.Destroy(skillStubs[i].renderObj);
                        skillStubs.RemoveAt(i);
                    }
                }
            }
            
        }
		
		public void use(KBEngine.Entity caster, SCObject target)
		{
			caster.cellCall("useTargetSkill", id, ((SCEntityObject)target).targetID);
            restCoolTimer = 0f;
		}
        public void cast(object renderObj, float distance)
        {
            SkillStub ss = new SkillStub();
            
            ss.renderObj = (UnityEngine.GameObject)renderObj;
            ss.skillDistance = distance;
            ss.isCasting = true;
            skillStubs.Add(ss);
        }
    }
} 
