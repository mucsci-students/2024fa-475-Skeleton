So I decided on implementing the enemies as a state machine- this means basically two scripts for each state
We also need a general state object and enemy implementation, so I called those EnemyState.cs and Enemy.cs
Enemies can have the following states: creating a script for each defines the behavior to be taken
1) IdleEnemyState
2) PatrolEnemyState
3) CombatEnemyState

Where each state corresponds to a specialized prefab based on the original enemy prefab whose names are
1) StationaryEnemy
2) PatrollingEnemy
3) CombativeEnemy
so we create scripts for each of these to define their instantiation 