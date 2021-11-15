<template>
	<v-card class="mx-auto">
		<v-card-title>
			<h2>使用者資料</h2>
		</v-card-title>
		<v-divider></v-divider>
		<v-card-text>
			<v-form>
				<v-text-field
					label="帳號"
					v-model="username"
					disabled
				/>
				<v-text-field
					label="姓名"
					v-model="fullName"
					disabled
				/>
				<v-text-field
					label="身分證字號"
					v-model="idNumber"
					disabled
				/>
				<v-text-field
					label="電話"
					v-model="phoneNumber"
					disabled
				/>
			</v-form>
		</v-card-text>
		<v-card-actions class="justify-end">
			<v-btn
				color="cyan accent-4"
				text
			>
				<router-link to="/users">使用者列表</router-link>
			</v-btn>
		</v-card-actions>
	</v-card>
</template>

<script>
import service from "../utils/request";

export default {
	name: "Dashboard",
	data() {
		return {
			username: "",
			idNumber: "",
			phoneNumber: "",
			fullName: "",
		};
	},
	methods: {
		getCurrentUser: function () {
			const currentUser = localStorage.getItem("currentUser");
			service
				.get("/user")
				.then((data) =>
					data.filter((user) => user.username == currentUser)
				)
				.then((user) => {
					this.username = user[0].username;
					this.idNumber = user[0].idNumber;
					this.phoneNumber = user[0].phoneNumber;
					this.fullName = user[0].fullName;
				});
		},
	},
	beforeMount() {
		this.getCurrentUser();
	},
};
</script>

<style lang="scss" scoped>
.v-card {
	min-width: 400px;
}
</style>