using System;

namespace ProjectTD.scripts.turrets {
public enum TurretSelectionStrategy {
	NEAREST,
	FURTHEST,
	FIRST,
	LAST,
	MIN_HEALTH,
	MAX_HEALTH
}

static class TurretSelectionStrategies {
	public static Result Calculate(this TurretSelectionStrategy strategy, Enemy current, Enemy toCheck,
		float currentData, float distance) {
		switch (strategy) {
			case TurretSelectionStrategy.NEAREST:
				return current == null || currentData > distance
					? new Result {enemy = toCheck, data = distance}
					: new Result {enemy = current, data = currentData};
			case TurretSelectionStrategy.FURTHEST:
				return current == null || currentData <= distance
					? new Result {enemy = toCheck, data = distance}
					: new Result {enemy = current, data = currentData};
			case TurretSelectionStrategy.FIRST:
				float firstProgress = toCheck._rotatingPathFollow.UnitOffset;
				return current == null || currentData <= firstProgress
					? new Result {enemy = toCheck, data = firstProgress}
					: new Result {enemy = current, data = currentData};
			case TurretSelectionStrategy.LAST:
				float lastProgress = toCheck._rotatingPathFollow.UnitOffset;
				return current == null || currentData > lastProgress
					? new Result {enemy = toCheck, data = lastProgress}
					: new Result {enemy = current, data = currentData};
			case TurretSelectionStrategy.MIN_HEALTH:
				float minHealth = toCheck.health;
				return current == null || currentData > minHealth
					? new Result {enemy = toCheck, data = minHealth}
					: new Result {enemy = current, data = currentData};
			case TurretSelectionStrategy.MAX_HEALTH:
				float maxHealth = toCheck.health;
				return current == null || currentData <= maxHealth
					? new Result {enemy = toCheck, data = maxHealth}
					: new Result {enemy = current, data = currentData};
			default:
				return new Result {enemy = toCheck, data = distance};
		}
	}
}

struct Result {
	public Enemy enemy { get; set; }
	public float data { get; set; }
}
}