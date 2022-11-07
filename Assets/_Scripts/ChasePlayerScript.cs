using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChasePlayerScript : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //ClockController clockController = animator.gameObject.GetComponent<ClockController>();
        NavMeshAgent navMeshAgent = animator.gameObject.GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(GameObject.Find("Player").transform.position);
        navMeshAgent.speed=10;
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		NavMeshAgent navMeshAgent = animator.gameObject.GetComponent<NavMeshAgent>();
        	navMeshAgent.SetDestination(GameObject.Find("Player").transform.position);
        	navMeshAgent.speed++;
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        NavMeshAgent navMeshAgent = animator.gameObject.GetComponent<NavMeshAgent>();
        navMeshAgent.speed = 5;
    }

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
