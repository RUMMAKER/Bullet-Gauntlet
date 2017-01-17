using UnityEngine;
using System.Collections;

public static class ParticleFactory {
	private static GameObject particlePrefab = Resources.Load<GameObject>("Prefabs/Effects/Particle");
	private static GameObject hollowParticlePrefab = Resources.Load<GameObject>("Prefabs/Effects/HollowParticle");
	private static GameObject flarePrefab = Resources.Load<GameObject>("Prefabs/Effects/Flare");
	private static GameObject explosionPrefab = Resources.Load<GameObject>("Prefabs/Effects/Explosion");

	public static GameObject MakeParticle(float startSpeed, float endSpeed, float startSize, float endSize, float lifeTime, Color startColor, Color endColor){
		GameObject particle = GameObject.Instantiate(particlePrefab);
		ParticleScript ps = particle.GetComponent<ParticleScript>();
		ps.SetAll(startSpeed, endSpeed, startSize, endSize, lifeTime, startColor, endColor);
		return particle;
	}
	public static GameObject MakeHollowParticle(float startSpeed, float endSpeed, float startSize, float endSize, float lifeTime, Color startColor, Color endColor){
		GameObject particle = GameObject.Instantiate(hollowParticlePrefab);
		ParticleScript ps = particle.GetComponent<ParticleScript>();
		ps.SetAll(startSpeed, endSpeed, startSize, endSize, lifeTime, startColor, endColor);
		return particle;
	}
	public static GameObject MakeFlare(float startSize, float endSize, float lifeTime, Color startColor, Color endColor){
		GameObject flare = GameObject.Instantiate(flarePrefab);
		ParticleScript ps = flare.GetComponent<ParticleScript>();
		ps.SetAll(0f, 0f, startSize, endSize, lifeTime, startColor, endColor);
		return flare;
	}
	public static GameObject MakeExplosion(float size){
		GameObject ex = GameObject.Instantiate(explosionPrefab);
		Explosion ts = ex.GetComponent<Explosion>();
		ts.SetSize(size);
		return ex;
	}
}
