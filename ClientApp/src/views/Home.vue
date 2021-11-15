<template>
	<v-card class="mx-auto">
		<v-card-title>
			<h2>請登入</h2>
		</v-card-title>
		<v-divider></v-divider>
		<v-card-text>
			<v-form>
				<v-text-field
					label="使用者"
					v-model="username"
					prepend-icon="mdi-account-circle"
				/>
				<v-text-field
					:type="isPasswordShown ? 'text' : 'password'"
					label="密碼"
					v-model="password"
					prepend-icon="mdi-lock"
					append-icon="mdi-eye-off"
					@click:append="isPasswordShown = !isPasswordShown"
				/>
			</v-form>
		</v-card-text>
		<v-divider></v-divider>
		<v-card-actions class="justify-end">
			<v-btn
				color="info"
				@click="submit()"
			>登入</v-btn>
		</v-card-actions>
	</v-card>
</template>

<script>
// @ is an alias to /src
import service from "../utils/request";

export default {
	name: "Home",
	data() {
		return {
			isPasswordShown: false,
			username: "",
			password: "",
		};
	},
	methods: {
		submit: function () {
			if (this.username != "" && this.password != "") {
				service
					.post("/auth/login", {
						username: this.username,
						password: this.password,
					})
					.then(() => {
						localStorage.setItem("currentUser", this.username);
						this.$router.push("/dashboard");
					})
					.catch((err) => console.log(err));
			} else {
				return false;
			}
		},
	},
};
</script>

<style lang="scss" scoped>
.v-card {
	min-width: 400px;
}
</style>
