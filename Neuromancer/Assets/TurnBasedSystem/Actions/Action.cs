using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedSystem {
	public interface Action {

		Character TakenBy();
		bool Execute();

	}

}