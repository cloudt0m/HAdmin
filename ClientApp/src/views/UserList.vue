<template>
	<v-data-table
		:headers="headers"
		:items="users"
		:item-per-page="5"
		class="elevation-1"
	>
	</v-data-table>
</template>

<script>
import service from "../utils/request";

export default {
	name: "User",
	data() {
		return {
			users: [],
			headers: [
				{ text: "使用者名稱", value: "username" },
				{ text: "身分證字號", value: "idNumber" },
				{ text: "電話", value: "phoneNumber" },
			],
		};
	},
	methods: {
		getUsers: function () {
			service
				.get("/user")
				.then((users) => (this.users = users))
				.catch((err) => console.log(err));
		},
	},
	mounted() {
		this.getUsers();
	},
};
</script>
